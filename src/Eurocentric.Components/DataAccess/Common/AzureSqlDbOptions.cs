namespace Eurocentric.Components.DataAccess.Common;

/// <summary>
///     Contains options for connecting to an Azure SQL database.
/// </summary>
internal sealed record AzureSqlDbOptions
{
    /// <summary>
    ///     Gets or sets the database connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the inclusive maximum number of retries.
    /// </summary>
    public int MaxRetries { get; set; }

    /// <summary>
    ///     Gets or sets the command timeout duration in seconds.
    /// </summary>
    public int CommandTimeoutInSeconds { get; set; }
}
