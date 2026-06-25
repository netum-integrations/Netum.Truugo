using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.ValidatorListProfiles.Definitions;
using Netum.Truugo.ValidatorListProfiles.Helpers;
using Newtonsoft.Json.Linq;

namespace Netum.Truugo.ValidatorListProfiles;

/// <summary>
/// Task Class for TruugoValidator operations.
/// </summary>
public static class Truugo
{
    /// <summary>
    /// Truugo API Validator list-profiles endpoint
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends-TruugoValidator-ListProfiles)
    /// </summary>
    /// <param name="connection">Connection parameters.</param>
    /// <param name="options">Additional error handling parameters.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, object[] Profile { string ProfileKey }, int StatusCode, string Endpoint, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static async Task<Result> ValidatorListProfiles(
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            string apiUrl = "https://api.truugo.com/validator/list-profiles";

            ValidationHandler.Run(connection);

            return await HandleRequest(
                     cancellationToken: cancellationToken,
                     url: apiUrl,
                     username: connection.Username,
                     password: connection.Password,
                     method: HttpMethod.Get);
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
        }
    }

    private static async Task<Result> HandleRequest(
        string url,
        string username,
        string password,
        HttpMethod method,
        CancellationToken cancellationToken)
    {
        try
        {
            string auth = $"{username}:{password}";
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

            var request = new HttpRequestMessage(method, url);

            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            JObject responseJson = JObject.Parse(responseContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API call failed with status code {response.StatusCode}: {responseContent}");
            }

            // Parse API response to extract profile keys and full profile objects
            var profiles = new List<Profile>();
            try
            {
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    using JsonDocument doc = JsonDocument.Parse(responseContent);

                    if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                        doc.RootElement.TryGetProperty("profiles", out JsonElement profilesArray) &&
                        profilesArray.ValueKind == JsonValueKind.Array)
                    {
                        var profilesJson = responseJson["profiles"] as JArray ?? new JArray();
                        int index = 0;
                        foreach (JsonElement profile in profilesArray.EnumerateArray())
                        {
                            if (profile.TryGetProperty("profile_key", out JsonElement keyElement))
                            {
                                var key = keyElement.GetString();
                                if (!string.IsNullOrEmpty(key))
                                {
                                    profiles.Add(new Profile
                                    {
                                        ProfileKey = key,
                                    });
                                }
                            }

                            index++;
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                throw new Exception($"JSON parsing failed: {ex.Message}. Response: {responseContent}", ex);
            }

            return new Result
            {
                Success = response.IsSuccessStatusCode,
                Profiles = profiles,
                StatusCode = (int)response.StatusCode,
                Endpoint = url,
                Error = null,
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}", ex);
        }
    }
}
