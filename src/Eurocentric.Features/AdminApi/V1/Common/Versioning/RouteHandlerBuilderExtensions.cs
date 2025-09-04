using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Features.AdminApi.V1.Common.Versioning;

/// <summary>
///     Extension methods to be invoked when configuring endpoints.
/// </summary>
internal static class RouteHandlerBuilderExtensions
{
    /// <summary>
    ///     Indicates that API version 1.0 is supported by the configured endpoint.
    /// </summary>
    /// <param name="builder">The endpoint route handler builder.</param>
    /// <returns>The same <see cref="RouteHandlerBuilder" /> instance, so that method invocations can be chained.</returns>
    internal static RouteHandlerBuilder IntroducedInVersion1Point0(this RouteHandlerBuilder builder) =>
        builder.HasApiVersion(1, 0);
}
