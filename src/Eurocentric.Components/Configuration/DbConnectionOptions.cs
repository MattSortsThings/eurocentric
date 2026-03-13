namespace Eurocentric.Components.Configuration;

/// <summary>
///     Contains options for connecting to the application database.
/// </summary>
public sealed record DbConnectionOptions
{
    public const string AppSettingsKey = "DbConnection";

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

    public void Deconstruct(out string connectionString, out int commandTimeoutInSeconds, out int maxRetries)
    {
        connectionString = ConnectionString;
        commandTimeoutInSeconds = CommandTimeoutInSeconds;
        maxRetries = MaxRetries;
    }
}
