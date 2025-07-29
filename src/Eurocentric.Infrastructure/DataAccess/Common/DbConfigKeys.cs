namespace Eurocentric.Infrastructure.DataAccess.Common;

internal static class DbConfigKeys
{
    internal static class ConnectionStrings
    {
        internal const string AzureSql = "AzureSql";
    }

    internal static class DbConnection
    {
        internal const string CommandTimeoutInSeconds = "DbConnection:CommandTimeoutInSeconds";
        internal const string MaxRetryCount = "DbConnection:MaxRetryCount";
    }
}
