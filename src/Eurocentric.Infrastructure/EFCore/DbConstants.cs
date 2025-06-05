namespace Eurocentric.Infrastructure.EFCore;

internal static class DbConstants
{
    internal const string SchemaName = "euro";
    internal const string ConnectionStringKey = "AzureSql";

    internal static class TableNames
    {
        internal const string Country = "country";
        internal const string CountryParticipatingContest = "country_participating_contest";
        internal const string EfCoreMigration = "ef_core_migration";
    }
}
