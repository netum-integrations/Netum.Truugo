using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netum.Truugo.ValidatorListProfiles.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// API Credentials username
    /// </summary>
    /// <example>api-user</example>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// API Credentials password
    /// </summary>
    /// <example>api-password</example>
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}