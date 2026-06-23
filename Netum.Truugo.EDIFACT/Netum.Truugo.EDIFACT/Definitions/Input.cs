using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netum.Truugo.EDIFACT.Definitions;

/// <summary>
/// TODO: Add summary.
/// </summary>
public enum FileImport
{
    /// <summary>
    /// Import file via file path
    /// </summary>
    FilePath,

    /// <summary>
    /// Import file via content
    /// </summary>
    FileContent,
}

/// <summary>
/// TODO: Add summary.
/// </summary>
public enum EndpointPath
{
    /// <summary>
    /// TODO: Add summary.
    /// </summary>
    CheckSyntax,

    /// <summary>
    /// TODO: Add summary.
    /// </summary>
    ToXML,

    /// <summary>
    /// TODO: Add summary.
    /// </summary>
    ToJSON,

    /// <summary>
    /// TODO: Add summary.
    /// </summary>
    GetBrowser,

    /// <summary>
    /// TODO: Add summary.
    /// </summary>
    GetDocumentedSample,
}

/// <summary>
/// TODO: Add summary.
/// </summary>
public class Input
{
    /// <summary>
    /// Path for the API call
    /// </summary>
    /// <example>CheckSyntax</example>
    [DisplayName("Select path")]
    public EndpointPath Endpoint { get; set; } = EndpointPath.CheckSyntax;

    /// <summary>
    /// File import type selection: file path or content
    /// </summary>
    /// <example>FilePath</example>
    [DisplayName("File import type")]
    public FileImport FileImportType { get; set; } = FileImport.FilePath;

    /// <summary>
    /// get-browser API call can store the report for 1-24 hrs
    /// </summary>
    /// <example>2</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.GetBrowser)]
    [DisplayName("Storage timespan")]
    public int StorageTime { get; set; }

    /// <summary>
    /// Full path to the file
    /// </summary>
    /// <example>C:\files\document.edi</example>
    [UIHint(nameof(FileImportType), "", FileImport.FilePath)]
    [DisplayName("Full path to the file")]
    [DisplayFormat(DataFormatString = "Text")]
    public string FilePath { get; set; }

    /// <summary>
    /// Name of the file, necessary when file content is used as input.
    /// </summary>
    /// <example>document.edi</example>
    [UIHint(nameof(FileImportType), "", FileImport.FileContent)]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("file.txt")]
    public string FileName { get; set; }

    /// <summary>
    /// Content to be validated.
    /// </summary>
    /// <example>UNB+UNOC:3+...</example>
    [UIHint(nameof(FileImportType), "", FileImport.FileContent)]
    [DisplayFormat(DataFormatString = "Text")]
    public string Content { get; set; }
}