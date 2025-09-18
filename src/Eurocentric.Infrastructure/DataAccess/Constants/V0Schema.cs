namespace Eurocentric.Infrastructure.DataAccess.Constants;

internal static class V0Schema
{
    internal const string Name = "v0";

    internal static class Tables
    {
        internal const string Broadcast = "broadcast";
        internal const string ChildBroadcast = "child_broadcast";
        internal const string Competitor = "competitor";
        internal const string Contest = "contest";
        internal const string ContestRole = "contest_role";
        internal const string Country = "country";
        internal const string Jury = "jury";
        internal const string JuryAward = "jury_award";
        internal const string Participant = "participant";
        internal const string Televote = "televote";
        internal const string TelevoteAward = "televote_award";
    }

    internal static class Sprocs
    {
        internal const string GetCompetingCountryPointsInRangeRankings = "v0.usp_get_competing_country_points_in_range_rankings";
        internal const string GetScoreboardRows = "v0.usp_get_scoreboard_rows";
    }
}
