namespace Eurocentric.Apis.Admin.V0.Constants;

internal static class V0Group
{
    internal const string Name = "AdminApi.V0";

    internal static class Countries
    {
        internal const string Tag = "Countries";

        internal static class Endpoints
        {
            internal const string CreateCountry = "AdminApi.V0.Countries.CreateCountry";
            internal const string GetCountries = "AdminApi.V0.Countries.GetCountries";
            internal const string GetCountry = "AdminApi.V0.Countries.GetCountry";
        }
    }
}
