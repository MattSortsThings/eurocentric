namespace Eurocentric.Components.DataAccess.Common;

/// <summary>
///     Contains options for connecting to the application database.
/// </summary>
internal sealed record DbConnectionOptions
{
    /// <summary>
    ///     Gets or sets the database connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the command timeout in seconds.
    /// </summary>
    public int CommandTimeoutInSeconds { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of retries.
    /// </summary>
    public int MaxRetries { get; set; }
}
