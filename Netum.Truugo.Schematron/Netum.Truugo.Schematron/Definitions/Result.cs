namespace Netum.Truugo.Schematron.Definitions;

/// <summary>
/// Result of the task.
/// </summary>
public class Result
{
    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Full response
    /// </summary>
    /// <example>responseObject { ... }</example>
    public dynamic Data { get; set; }

    /// <summary>
    /// HTTP status code returned by the API.
    /// </summary>
    /// <example>200</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Endpoint used in the API call
    /// </summary>
    /// <example>https://api.truugo.com/schematron/validate</example>
    public string Endpoint { get; set; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}
