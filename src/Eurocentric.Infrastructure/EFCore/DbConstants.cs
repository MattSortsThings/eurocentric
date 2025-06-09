namespace Eurocentric.Infrastructure.EFCore;

internal static class DbConstants
{
    internal const string SchemaName = "euro";
    internal const string ConnectionStringKey = "AzureSql";

    internal static class TableNames
    {
        internal const string Broadcast = "broadcast";
        internal const string BroadcastCompetitor = "broadcast_competitor";
        internal const string BroadcastCompetitorJuryAward = "broadcast_competitor_jury_award";
        internal const string BroadcastCompetitorTelevoteAward = "broadcast_competitor_televote_award";
        internal const string BroadcastJury = "broadcast_jury";
        internal const string BroadcastTelevote = "broadcast_televote";
        internal const string Contest = "contest";
        internal const string ContestChildBroadcast = "contest_child_broadcast";
        internal const string ContestParticipant = "contest_participant";
        internal const string Country = "country";
        internal const string CountryParticipatingContest = "country_participating_contest";
        internal const string EfCoreMigration = "ef_core_migration";
    }
}
