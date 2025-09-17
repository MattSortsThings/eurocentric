namespace Eurocentric.Infrastructure.DataAccess.Constants;

public static class DbConfigKeys
{
    public const string ConnectionString = "AzureSql";
    public const string MaxRetryCount = "AzureDbConnection:MaxRetryCount";
    public const string CommandTimeoutInSeconds = "AzureDbConnection:CommandTimeoutInSeconds";
}
