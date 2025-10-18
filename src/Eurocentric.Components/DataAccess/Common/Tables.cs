namespace Eurocentric.Components.DataAccess.Common;

internal static class Tables
{
    internal static class Dbo
    {
        internal const string EfMigrationsHistory = "__ef_migrations_history";
    }

    internal static class V0
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
        internal const string CountryContestRole = "country_contest_role";
    }
}
