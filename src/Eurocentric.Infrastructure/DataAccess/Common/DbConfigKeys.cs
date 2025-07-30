namespace Eurocentric.Infrastructure.DataAccess.Common;

public static class DbConfigKeys
{
    public static class ConnectionStrings
    {
        internal const string AzureSql = "AzureSql";
    }

    public static class DbConnection
    {
        internal const string CommandTimeoutInSeconds = "DbConnection:CommandTimeoutInSeconds";
        internal const string MaxRetryCount = "DbConnection:MaxRetryCount";
    }
}
