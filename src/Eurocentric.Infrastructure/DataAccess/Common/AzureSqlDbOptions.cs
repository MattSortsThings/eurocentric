namespace Eurocentric.Infrastructure.DataAccess.Common;

public sealed record AzureSqlDbOptions
{
    public int MaxRetryCount { get; init; }

    public int CommandTimeoutInSeconds { get; init; }
}
