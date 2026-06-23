using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netum.Truugo.Validator.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// API Credentials username
    /// </summary>
    /// <example>apiuser</example>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// API Credentials password
    /// </summary>
    /// <example>apipassword</example>
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }

    /// <summary>
    /// Profile key for API
    /// </summary>
    /// <example>FI-EN123456</example>
    [Required]
    public string ProfileKey { get; set; }
}
