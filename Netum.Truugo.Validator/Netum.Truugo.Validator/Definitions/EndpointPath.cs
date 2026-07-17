namespace Netum.Truugo.Validator.Definitions;

/// <summary>
/// Truugo API endpoint paths.
/// </summary>
public enum EndpointPath
{
    /// <summary>
    /// Get status endpoint validates given file against the selected test profile.
    /// </summary>
    GetStatus,

    /// <summary>
    /// Store status for 30 days endpoint validates given file against the selected test profile and stores the result for 30 days.
    /// </summary>
    StoreStatus30d,

    /// <summary>
    /// Store status for 60 days endpoint validates given file against the selected test profile and stores the result for 60 days.
    /// </summary>
    StoreStatus60d,

    /// <summary>
    /// Get feedback endpoint validates given file against the selected test profile and returns a test report in JSON.
    /// </summary>
    GetFeedback,

    /// <summary>
    /// Get report endpoint validates given file against the selected test profile and returns result as a code and URL pointing to the report.
    /// </summary>
    GetReport,

    /// <summary>
    /// Store report for 7 days endpoint validates given file against the selected test profile and returns result as a code and URL pointing to the report. The report is stored for 7 days.
    /// </summary>
    StoreReport7d,

    /// <summary>
    /// Store report for 14 days endpoint validates given file against the selected test profile and returns result as a code and URL pointing to the report. The report is stored for 14 days.
    /// </summary>
    StoreReport14d,

    /// <summary>
    /// Store report for 30 days endpoint validates given file against the selected test profile and returns result as a code and URL pointing to the report. The report is stored for 30 days.
    /// </summary>
    StoreReport30d,
}