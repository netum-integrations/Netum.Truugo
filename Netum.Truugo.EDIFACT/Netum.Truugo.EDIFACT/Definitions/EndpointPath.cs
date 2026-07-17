namespace Netum.Truugo.EDIFACT.Definitions;

/// <summary>
/// Endpoint path options for the API call.
/// </summary>
public enum EndpointPath
{
    /// <summary>
    /// CheckSyntax endpoint to validate the syntax of the EDIFACT message.
    /// </summary>
    CheckSyntax,

    /// <summary>
    /// ToXML endpoint to convert the EDIFACT message to XML.
    /// </summary>
    ToXML,

    /// <summary>
    /// ToJSON endpoint to convert the EDIFACT message to JSON.
    /// </summary>
    ToJSON,

    /// <summary>
    /// GetBrowser endpoint to visualize the file instance using the EDIFACT Browser feature.
    /// </summary>
    GetBrowser,

    /// <summary>
    /// GetDocumentedSample endpoint transforms the message structure and content to a CSV file, which can be formatted and visualized using a spreadsheet application.
    /// </summary>
    GetDocumentedSample,
}