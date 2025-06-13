namespace Eurocentric.Features.AdminApi.V1.Common.Constants;

internal static class EndpointIds
{
    internal static class Broadcasts
    {
        internal const string AwardJuryPoints = "AdminApi.V1.Broadcasts.AwardJuryPoints";
        internal const string AwardTelevotePoints = "AdminApi.V1.Broadcasts.AwardTelevotePoints";
        internal const string GetBroadcast = "AdminApi.V1.Broadcasts.GetBroadcast";
        internal const string GetBroadcasts = "AdminApi.V1.Broadcasts.GetBroadcasts";
    }

    internal static class Contests
    {
        internal const string CreateChildBroadcast = "AdminApi.V1.Contests.CreateChildBroadcast";
        internal const string CreateContest = "AdminApi.V1.Contests.CreateContest";
        internal const string GetContest = "AdminApi.V1.Contests.GetContest";
        internal const string GetContests = "AdminApi.V1.Contests.GetContests";
    }

    internal static class Countries
    {
        internal const string CreateCountry = "AdminApi.V1.Countries.CreateCountry";
        internal const string GetCountries = "AdminApi.V1.Countries.GetCountries";
        internal const string GetCountry = "AdminApi.V1.Countries.GetCountry";
    }
}
