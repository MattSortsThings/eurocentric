namespace Eurocentric.Infrastructure.DataAccess.Common;

public static class DbConfigKeys
{
    public static class ConnectionStrings
    {
        public const string AzureSql = "AzureSql";
    }

    public static class DbConnection
    {
        public const string CommandTimeoutInSeconds = "DbConnection:CommandTimeoutInSeconds";
        public const string MaxRetryCount = "DbConnection:MaxRetryCount";
    }
}
