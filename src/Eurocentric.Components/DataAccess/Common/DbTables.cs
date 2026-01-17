namespace Eurocentric.Components.DataAccess.Common;

internal static class DbTables
{
    internal static class Dbo
    {
        internal const string EfMigrationsHistory = "__ef_migrations_history";
    }

    internal static class Placeholder
    {
        internal const string Broadcast = "broadcast";
        internal const string BroadcastCompetitor = "broadcast_competitor";
        internal const string BroadcastCompetitorPointsAward = "broadcast_competitor_points_award";
        internal const string BroadcastJury = "broadcast_jury";
        internal const string BroadcastTelevote = "broadcast_televote";
        internal const string Contest = "contest";
        internal const string ContestBroadcastMemo = "contest_broadcast_memo";
        internal const string ContestParticipant = "contest_participant";
        internal const string Country = "country";
        internal const string CountryContestRole = "country_contest_role";
    }
}
