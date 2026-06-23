using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.EDIFACT.Definitions;
using Netum.Truugo.EDIFACT.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netum.Truugo.EDIFACT;

/// <summary>
/// Task class.
/// </summary>
public static class Truugo
{
    /// <summary>
    /// TruugoEDIFACT task for calling EDIFACT API endpoints. Depending on the selected endpoint, different parameters are required. Truugo API Swagger for reference: https://api.truugo.com/reference/
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends-TruugoEDIFACT-EDIFACT)
    /// </summary>
    /// <param name="input">Input parameters for API.</param>
    /// <param name="connection">Connection parameters.</param>
    /// <param name="options">Options for task execution.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>Result object {bool Success, dynamic Data, int StatusCode, string Endpoint, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static async Task<Result> EDIFACT([PropertyTab] Input input, [PropertyTab] Connection connection, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        try
        {
            string serverUrl = "https://api.truugo.com";
            string apiUrl;

            ValidationHandler.Run(connection);

            switch (input.Endpoint)
            {
                case EndpointPath.CheckSyntax:
                    apiUrl = serverUrl + "/edifact/check-syntax";
                    return await HandleRequest(
                        cancellationToken: cancellationToken,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        filePath: input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.Content);

                case EndpointPath.ToXML:
                    apiUrl = serverUrl + "/edifact/to-xml";
                    return await HandleRequest(
                        cancellationToken: cancellationToken,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        filePath: input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.Content);

                case EndpointPath.ToJSON:
                    apiUrl = serverUrl + "/edifact/to-json";
                    return await HandleRequest(
                        cancellationToken: cancellationToken,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        filePath: input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.Content);

                case EndpointPath.GetBrowser:
                    apiUrl = serverUrl + "/edifact/get-browser";
                    return await HandleRequest(
                        cancellationToken: cancellationToken,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        storageTime: input.StorageTime,
                        filePath: input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.Content);

                case EndpointPath.GetDocumentedSample:
                    apiUrl = serverUrl + "/edifact/get-documented-sample";
                    return await HandleRequest(
                        cancellationToken: cancellationToken,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        filePath: input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.Content);

                default:
                    apiUrl = serverUrl + "/edifact/check-syntax";
                    return await HandleRequest(
                        cancellationToken: cancellationToken,
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        filePath: input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.Content);
            }
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
        }
    }

    private static async Task<Result> HandleRequest(
        CancellationToken cancellationToken,
        string url,
        string username,
        string password,
        HttpMethod method,
        int storageTime = 24,
        string filePath = null,
        string fileName = null,
        string fileContents = null)
    {
        try
        {
            string auth = $"{username}:{password}";
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

            var request = new HttpRequestMessage(method, url);
            var form = new MultipartFormDataContent();

            ByteArrayContent fileContent;

            if (!string.IsNullOrEmpty(filePath))
            {
                fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                form.Add(fileContent, "instance", System.IO.Path.GetFileName(filePath));
            }
            else if (!string.IsNullOrEmpty(fileContents))
            {
                fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(fileContents));
                form.Add(fileContent, "instance", fileName);
            }
            else
            {
                throw new ArgumentException("Missing file path or file contents.");
            }

            if (url == "https://api.truugo.com/edifact/get-browser")
            {
                if (storageTime <= 0 || storageTime > 24)
                    throw new ArgumentOutOfRangeException(nameof(storageTime), $"Storage time must be greater than 0 and less than or equal to 24. Provided value: {storageTime}");

                form.Add(new StringContent(storageTime.ToString()), "storage_timespan");
            }

            request.Content = form;

            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
            dynamic responseContent = await response.Content.ReadAsStringAsync();
            JObject responseJson = JObject.Parse(responseContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API call failed with status code {response.StatusCode}: {responseContent}");
            }

            JArray decodedMessages = null;
            if (url == "https://api.truugo.com/edifact/to-xml")
            {
                var messages = responseJson["messages"] as JObject;
                if (messages != null && messages.Count > 0)
                {
                    decodedMessages = new JArray(
                        messages.Properties()
                            .OrderBy(p => p.Name)
                            .Select(p =>
                            {
                                var encoded = p.Value.Value<string>();
                                if (string.IsNullOrEmpty(encoded)) return null;
                                byte[] decodedBytes = Convert.FromBase64String(encoded);
                                return Encoding.UTF8.GetString(decodedBytes);
                            })
                            .Where(s => s != null));
                    if (decodedMessages.Count == 0) decodedMessages = null;
                }
            }

            if (decodedMessages != null)
            {
                return new Result
                {
                    Success = true,
                    Data = new JObject
                    {
                        ["Body"] = decodedMessages,
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
