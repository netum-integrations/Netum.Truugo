using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netum.Truugo.EDIFACT.Definitions;

/// <summary>
/// Input parameters for the API call.
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
    /// GetBrowser API call can store the report for 1-24 hrs
    /// </summary>
    /// <example>2</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.GetBrowser)]
    [DisplayName("Storage timespan")]
    public int StorageTimeInHours { get; set; }

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
    public string FileName { get; set; }

    /// <summary>
    /// Content to be validated.
    /// </summary>
    /// <example>UNB+UNOC:3+...</example>
    [UIHint(nameof(FileImportType), "", FileImport.FileContent)]
    [DisplayFormat(DataFormatString = "Text")]
    public string Content { get; set; }
}