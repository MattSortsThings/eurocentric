using Eurocentric.Apis.Admin.V0.Features.Countries;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Maps the version 0.x endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    public static void MapV0Endpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapCreateCountryV0Point1().MapGetCountryV0Point1();

        builder.MapCreateCountryV0Point2().MapGetCountriesV0Point2().MapGetCountryV0Point2();
    }
}
