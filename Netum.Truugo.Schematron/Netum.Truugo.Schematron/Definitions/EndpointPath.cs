namespace Netum.Truugo.Schematron.Definitions;

/// <summary>
/// Truugo API endpoint paths.
/// </summary>
public enum EndpointPath
{
    /// <summary>
    /// List items endpoint lists available Schematron items for a specific group key.
    /// </summary>
    ListItems,

    /// <summary>
    /// List item versions endpoint lists all available versions of a specific item key.
    /// </summary>
    ListItemVersions,

    /// <summary>
    /// Validate endpoint performs Schematron validation on the provided file instance.
    /// </summary>
    Validate,
}