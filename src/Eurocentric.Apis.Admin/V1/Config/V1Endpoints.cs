namespace Eurocentric.Apis.Admin.V1.Config;

internal static class V1Endpoints
{
    internal static class Broadcasts
    {
        internal const string GetBroadcast = "GetBroadcast";
        internal const string GetBroadcasts = "GetBroadcasts";
    }

    internal static class Contests
    {
        internal const string CreateContest = "AdminApi.V1.CreateContest";
        internal const string CreateContestBroadcast = "AdminApi.V1.CreateContestBroadcast";
        internal const string GetContest = "AdminApi.V1.GetContest";
        internal const string GetContests = "AdminApi.V1.GetContests";
    }

    internal static class Countries
    {
        internal const string CreateCountry = "AdminApi.V1.CreateCountry";
        internal const string GetCountries = "AdminApi.V1.GetCountries";
        internal const string GetCountry = "AdminApi.V1.GetCountry";
    }
}
