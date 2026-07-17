using System.Collections.Generic;

namespace Netum.Truugo.ValidatorListProfiles.Definitions;

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
    /// List of profiles extracted from the response. Each entry exposes the profile key via ProfileKey.
    /// </summary>
    /// <example>[{ "ProfileKey": "profile_key_1" }]</example>
    public List<Profile> Profiles { get; set; } = new List<Profile>();

    /// <summary>
    /// HTTP status code returned by the API.
    /// </summary>
    /// <example>200</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Endpoint used in the API call.
    /// </summary>
    /// <example>https://api.truugo.com/validator/list-profiles</example>
    public string Endpoint { get; set; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}
