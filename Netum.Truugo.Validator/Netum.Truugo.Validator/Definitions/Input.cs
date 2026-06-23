using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netum.Truugo.Validator.Definitions;

/// <summary>
/// File import choices.
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
/// Truugo API endpoint paths.
/// </summary>
public enum EndpointPath
{
    /// <summary>
    /// Get status endpoint
    /// </summary>
    GetStatus,

    /// <summary>
    /// Store status for 30 days endpoint
    /// </summary>
    StoreStatus30d,

    /// <summary>
    /// Store status for 60 days endpoint
    /// </summary>
    StoreStatus60d,

    /// <summary>
    /// Get feedback endpoint
    /// </summary>
    GetFeedback,

    /// <summary>
    /// Get report endpoint
    /// </summary>
    GetReport,

    /// <summary>
    /// Store report for 7 days endpoint
    /// </summary>
    StoreReport7d,

    /// <summary>
    /// Store report for 14 days endpoint
    /// </summary>
    StoreReport14d,

    /// <summary>
    /// Store report for 30 days endpoint
    /// </summary>
    StoreReport30d,
}

/// <summary>
/// Required and optional API parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Path for the API call
    /// </summary>
    /// <example>
    /// EndpointPath.GetStatus
    /// </example>
    public EndpointPath Endpoint { get; set; } = EndpointPath.GetStatus;

    /// <summary>
    /// File import type selection: file path or content
    /// </summary>
    /// <example>
    /// FileImport.FilePath
    /// </example>
    public FileImport FileImportType { get; set; } = FileImport.FilePath;

    /// <summary>
    /// Full path to the file to be validated
    /// </summary>
    /// <example>C:\files\report.xml</example>
    [UIHint(nameof(FileImportType), "", FileImport.FilePath)]
    [DisplayFormat(DataFormatString = "Text")]
    public string FilePath { get; set; }

    /// <summary>
    /// Name of the file, necessary when file content is used as input.
    /// </summary>
    /// <example>report.xml</example>
    [UIHint(nameof(FileImportType), "", FileImport.FileContent)]
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("file.xml")]
    public string FileName { get; set; }

    /// <summary>
    /// get-report API call can store the report for 1-24 hrs
    /// </summary>
    /// <example>24</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.GetReport)]
    [DefaultValue(24)]
    public int StorageTime { get; set; } = 24;

    /// <summary>
    /// Optional instance name, can be used to override the file name in statistics.
    /// </summary>
    /// <example>CustomInstanceName</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.StoreStatus30d, EndpointPath.StoreStatus60d, EndpointPath.GetFeedback, EndpointPath.GetReport, EndpointPath.StoreReport7d, EndpointPath.StoreReport14d, EndpointPath.StoreReport30d)]
    public string InstanceName { get; set; }

    /// <summary>
    /// Optional tag
    /// </summary>
    /// <example>CustomTag</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.StoreStatus30d, EndpointPath.StoreStatus60d, EndpointPath.GetReport, EndpointPath.StoreReport7d, EndpointPath.StoreReport14d, EndpointPath.StoreReport30d)]
    public string Tag { get; set; }

    /// <summary>
    /// Can be used to limit the feedback to invalid test phases only.
    /// </summary>
    /// <example>false</example>
    [UIHint(nameof(Endpoint), "", EndpointPath.GetFeedback)]
    [DefaultValue(false)]
    public bool ErrorsOnly { get; set; } = false;

    /// <summary>
    /// XML content to be validated.
    /// </summary>
    /// <example>&lt;report&gt;...&lt;/report&gt;</example>
    [UIHint(nameof(FileImportType), "", FileImport.FileContent)]
    [DisplayFormat(DataFormatString = "Text")]
    public string Content { get; set; }
}
