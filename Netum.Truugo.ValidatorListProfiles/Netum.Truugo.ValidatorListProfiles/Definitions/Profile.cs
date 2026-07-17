namespace Netum.Truugo.ValidatorListProfiles.Definitions;

/// <summary>
/// Represents a single profile returned by the Truugo Validator List-profiles endpoint.
/// </summary>
public class Profile
{
    /// <summary>
    /// The unique key identifying the profile.
    /// </summary>
    /// <example>profile_key_1</example>
    public string ProfileKey { get; set; }
}
