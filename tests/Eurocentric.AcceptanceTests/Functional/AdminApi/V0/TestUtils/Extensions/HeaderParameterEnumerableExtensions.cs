using RestSharp;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V0.TestUtils.Extensions;

public static class HeaderParameterEnumerableExtensions
{
    public static string ExtractLocation(this IEnumerable<HeaderParameter> headerParameters)
    {
        HeaderParameter locationParameter = headerParameters.Single(header => header.Name == "Location");

        return locationParameter.Value;
    }
}
