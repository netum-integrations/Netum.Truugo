using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netum.Truugo.Schematron.Definitions;

/// <summary>
/// Input parameters for the API call.
/// </summary>
public class Input
{
    /// <summary>
    /// Path for the API call
    /// </summary>
    /// <example>ListItems</example>
    [DisplayName("Select path")]
    public EndpointPath Endpoint { get; set; } = EndpointPath.ListItems;

    /// <summary>
    /// Group key needed for Schematron List-Items
    /// </summary>
    /// <example>groupKey</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.ListItems)]
    [DisplayName("Group key")]
    public string GroupKey { get; set; }

    /// <summary>
    /// Item key needed for Schematron List-Item-Versions
    /// </summary>
    /// <example>itemKey</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.ListItemVersions)]
    [DisplayName("Item key")]
    public string ItemKey { get; set; }

    /// <summary>
    /// File key needed for Schematron Validate
    /// </summary>
    /// <example>fileKey</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.Validate)]
    [DisplayName("File key")]
    public string FileKey { get; set; }

    /// <summary>
    /// File import type selection: file path or content
    /// </summary>
    /// <example>FilePath</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.Validate)]
    public FileImport FileImportType { get; set; }

    /// <summary>
    /// Full path to the file, only needed if file import type is set to file path.
    /// </summary>
    /// <example>C:\files\file.xml</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.Validate)]
    [DisplayFormat(DataFormatString = "Text")]
    public string FilePath { get; set; }

    /// <summary>
    /// Name of the file, necessary when file content is used as input. Can also override the file name when used with file path.
    /// </summary>
    /// <example>file.xml</example>
    [UIHint(nameof(FileImportType), "", FileImport.FileContent)]
    [DisplayFormat(DataFormatString = "Text")]
    public string FileName { get; set; }

    /// <summary>
    /// XML content to be validated.
    /// </summary>
    /// <example>&lt;xml&gt;...&lt;/xml&gt;</example>
    [UIHint(nameof(FileImportType), "", FileImport.FileContent)]
    [DisplayFormat(DataFormatString = "Text")]
    public string Content { get; set; }
}