using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.Schematron.Definitions;
using Netum.Truugo.Schematron.Helpers;
using Newtonsoft.Json.Linq;

namespace Netum.Truugo.Schematron;

/// <summary>
/// Task Class for Truugo operations.
/// </summary>
public static class Truugo
{
    /// <summary>
    /// Task for calling Truugo Schematron API endpoints and returns its response. Truugo API Swagger for reference: https://api.truugo.com/reference/
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Netum-Truugo-Schematron)
    /// </summary>
    /// <param name="input">Essential parameters.</param>
    /// <param name="connection">Connection parameters.</param>
    /// <param name="options">Additional parameters.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>Result object {bool Success, dynamic Data, int StatusCode, string Endpoint, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static async Task<Result> Schematron([PropertyTab] Input input, [PropertyTab] Connection connection, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        try
        {
            string serverUrl = "https://api.truugo.com";
            string apiUrl;

            ValidationHandler.Run(connection);

            switch (input.Endpoint)
            {
                case EndpointPath.ListItems:
                    apiUrl = serverUrl + $"/schematron/list-items?group_key={input.GroupKey}";
                    return await HandleRequest(
                        endpoint: input.Endpoint,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Get,
                        cancellationToken: cancellationToken);

                case EndpointPath.ListItemVersions:
                    apiUrl = serverUrl + $"/schematron/list-item-versions?item_key={input.ItemKey}";
                    return await HandleRequest(
                        endpoint: input.Endpoint,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Get,
                        cancellationToken: cancellationToken);

                case EndpointPath.Validate:
                    apiUrl = serverUrl + "/schematron/validate";
                    return await HandleRequest(
                        endpoint: input.Endpoint,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        fileKey: input.FileKey,
                        filePath: input.FileImportType == FileImport.FilePath ? input.FilePath : null,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FileContent ? input.Content : null,
                        cancellationToken: cancellationToken);

                default:
                    apiUrl = serverUrl + $"/schematron/list-items?group_key={input.GroupKey}";
                    return await HandleRequest(
                        endpoint: input.Endpoint,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Get,
                        cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
        }
    }

    private static async Task<Result> HandleRequest(
        EndpointPath endpoint,
        string url,
        string username,
        string password,
        HttpMethod method,
        string fileKey = null,
        string filePath = null,
        string fileName = null,
        string fileContents = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            string auth = $"{username}:{password}";
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

            using var request = new HttpRequestMessage(method, url);

            if (method == HttpMethod.Post && !string.IsNullOrEmpty(fileKey))
            {
                var form = new MultipartFormDataContent
                {
                    { new StringContent(fileKey), "file_key" },
                };

                if (!string.IsNullOrEmpty(filePath))
                {
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    form.Add(fileContent, "instance", System.IO.Path.GetFileName(filePath));
                }
                else if (!string.IsNullOrEmpty(fileContents))
                {
                    var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(fileContents));
                    form.Add(fileContent, "instance", fileName);
                }
                else
                {
                    throw new ArgumentException("Missing file path or file contents.");
                }

                request.Content = form;
            }

            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API call failed with status code {response.StatusCode}: {responseContent}");
            }

            JObject responseJson = JObject.Parse(responseContent);

            string decodedContent = null;
            if (endpoint == EndpointPath.Validate)
            {
                var encodedContent = responseJson["svrl"]?.Value<string>();
                if (!string.IsNullOrEmpty(encodedContent))
                {
                    byte[] decodedBytes = Convert.FromBase64String(encodedContent);
                    decodedContent = Encoding.UTF8.GetString(decodedBytes);
                }

                return new Result
                {
                    Success = response.IsSuccessStatusCode,
                    Data = new JObject
                    {
                        ["Body"] = decodedContent,
                        ["Price"] = responseJson["price"]?.Value<string>(),
                    },
                    StatusCode = (int)response.StatusCode,
                    Endpoint = url,
                };
            }
            else
            {
                return new Result
                {
                    Success = response.IsSuccessStatusCode,
                    Data = responseJson,
                    StatusCode = (int)response.StatusCode,
                    Endpoint = url,
                };
            }
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
