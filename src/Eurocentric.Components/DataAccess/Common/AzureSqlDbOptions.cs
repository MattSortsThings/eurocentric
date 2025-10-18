namespace Eurocentric.Components.DataAccess.Common;

/// <summary>
///     Contains options used for connecting to the Azure SQL database.
/// </summary>
public sealed record AzureSqlDbOptions
{
    /// <summary>
    ///     Gets the command timeout in seconds.
    /// </summary>
    public int CommandTimeoutInSeconds { get; set; }

    /// <summary>
    ///     Gets the database connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///     Gets the maximum number of retry attempts.
    /// </summary>
    public int MaxRetries { get; set; }

    /// <summary>
    ///     Gets the time in seconds that should elapse before retrying an HTTP request that failed due to the database being
    ///     offline.
    /// </summary>
    public int HttpRetryAfterSeconds { get; set; }
}
