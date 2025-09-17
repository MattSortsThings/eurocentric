using Eurocentric.Apis.Admin.V0.Features.Countries;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin.V0.Features;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the v0.x endpoints to the route builder.
    /// </summary>
    /// <param name="builder">The route builder.</param>
    internal static void MapV0Endpoints(this IEndpointRouteBuilder builder) => builder.MapCreateCountryV0Point1()
        .MapGetCountryV0Point1();
}
