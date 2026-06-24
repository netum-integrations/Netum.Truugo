using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Netum.Truugo.Validator.Definitions;
using Netum.Truugo.Validator.Helpers;
using Newtonsoft.Json.Linq;

namespace Netum.Truugo.Validator;

/// <summary>
/// Task Class for Truugo operations.
/// </summary>
public static class Truugo
{
    /// <summary>
    /// Task for calling specified Truugo API Validator endpoint and returns its response. Truugo API Swagger for reference: https://api.truugo.com/reference/
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Netum-Truugo-Validator)
    /// </summary>
    /// <param name="input">Input parameters for API.</param>
    /// <param name="connection">Connection parameters.</param>
    /// <param name="options">Options parameters.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>Result object {bool Success, dynamic Data, int StatusCode, string Endpoint, object Error { string Message, Exception AdditionalInfo }}</returns>
    public static async Task<Result> Validator([PropertyTab] Input input, [PropertyTab] Connection connection, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        try
        {
            string serverUrl = "https://api.truugo.com";
            string apiUrl;

            ValidationHandler.Run(connection);

            switch (input.Endpoint)
            {
                case EndpointPath.GetStatus:
                    apiUrl = serverUrl + "/validator/get-status";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        cancellationToken: cancellationToken);

                case EndpointPath.StoreStatus30d:
                    apiUrl = serverUrl + "/validator/store-status-30-d";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        instance_name: input.InstanceName,
                        tag: input.Tag,
                        cancellationToken: cancellationToken);

                case EndpointPath.StoreStatus60d:
                    apiUrl = serverUrl + "/validator/store-status-60-d";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        instance_name: input.InstanceName,
                        tag: input.Tag,
                        cancellationToken: cancellationToken);

                case EndpointPath.GetFeedback:
                    apiUrl = serverUrl + "/validator/get-feedback";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        instance_name: input.InstanceName,
                        errors_only: input.ErrorsOnly,
                        cancellationToken: cancellationToken);

                case EndpointPath.GetReport:
                    apiUrl = serverUrl + "/validator/get-report";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        storageTime: input.StorageTime,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        instance_name: input.InstanceName,
                        tag: input.Tag,
                        cancellationToken: cancellationToken);

                case EndpointPath.StoreReport7d:
                    apiUrl = serverUrl + "/validator/store-report-7-d";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        instance_name: input.InstanceName,
                        tag: input.Tag,
                        cancellationToken: cancellationToken);

                case EndpointPath.StoreReport14d:
                    apiUrl = serverUrl + "/validator/store-report-14-d";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        instance_name: input.InstanceName,
                        tag: input.Tag,
                        cancellationToken: cancellationToken);

                case EndpointPath.StoreReport30d:
                    apiUrl = serverUrl + "/validator/store-report-30-d";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.Content,
                        instance_name: input.InstanceName,
                        tag: input.Tag,
                        cancellationToken: cancellationToken);

                default:
                    apiUrl = serverUrl + "/validator/get-status";
                    return await HandleRequest(
                        url: apiUrl,
                        username: connection.Username,
                        password: connection.Password,
                        method: HttpMethod.Post,
                        key: connection.ProfileKey,
                        filePath: input.FileImportType == FileImport.FileContent ? null : input.FilePath,
                        fileName: input.FileName,
                        fileContents: input.FileImportType == FileImport.FilePath ? null : input.Content,
                        cancellationToken: cancellationToken);
            }
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
        string key = null,
        int storageTime = 24,
        string filePath = null,
        string fileName = null,
        string fileContents = null,
        string instance_name = null,
        string tag = null,
        bool errors_only = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            string auth = $"{username}:{password}";
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrEmpty(key))
            {
                var form = new MultipartFormDataContent { { new StringContent(key), "profile_key" } };

                ByteArrayContent fileContent;

                if (!string.IsNullOrEmpty(filePath))
                {
                    fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    form.Add(fileContent, "instance", System.IO.Path.GetFileName(filePath));

                    // Optionals
                    if (!string.IsNullOrEmpty(instance_name))
                        form.Add(new StringContent(instance_name), "instance_name");
                    if (!string.IsNullOrEmpty(tag))
                        form.Add(new StringContent(tag), "tag");
                }
                else if (!string.IsNullOrEmpty(fileContents))
                {
                    fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(fileContents));
                    form.Add(fileContent, "instance", fileName);

                    // Optionals
                    if (!string.IsNullOrEmpty(instance_name))
                        form.Add(new StringContent(instance_name), "instance_name");
                    if (!string.IsNullOrEmpty(tag))
                        form.Add(new StringContent(tag), "tag");
                }
                else
                {
                    throw new ArgumentException("Missing file path or file contents.");
                }

                if (url == "https://api.truugo.com/validator/get-report")
                {
                    if (storageTime <= 0 || storageTime > 24)
                        throw new ArgumentOutOfRangeException(nameof(storageTime), $"Storage time must be greater than 0 and less than or equal to 24. Provided value: {storageTime}");

                    form.Add(new StringContent(storageTime.ToString()), "storage_timespan");
                }

                if (errors_only == true)
                    form.Add(new StringContent("1"), "errors_only");
                else
                    form.Add(new StringContent("0"), "errors_only");

                request.Content = form;
            }

            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
            string responseContent = await response.Content.ReadAsStringAsync();
            JObject responseJson = JObject.Parse(responseContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API call failed with status code {response.StatusCode}: {responseContent}");
            }

            return new Result
            {
                Success = response.IsSuccessStatusCode,
                Data = responseJson,
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
