using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netum.Truugo.Schematron.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// API Credentials username
    /// </summary>
    /// <example>APIuser</example>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// API Credentials password
    /// </summary>
    /// <example>APIPassword</example>
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}
