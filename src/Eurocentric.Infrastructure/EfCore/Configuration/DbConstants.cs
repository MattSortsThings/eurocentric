namespace Eurocentric.Infrastructure.EfCore.Configuration;

internal static class DbConstants
{
    internal const string ConnectionStringKey = "AzureSql";

    internal static class TableNames
    {
        internal const string EfCoreMigration = "ef_core_migration";
        internal const string Country = "country";
        internal const string CountryContestMemo = "country_contest_memo";
    }
}
