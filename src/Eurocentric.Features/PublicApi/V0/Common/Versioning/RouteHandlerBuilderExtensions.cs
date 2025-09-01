using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Features.PublicApi.V0.Common.Versioning;

/// <summary>
///     Extension methods to be invoked when configuring endpoints.
/// </summary>
internal static class RouteHandlerBuilderExtensions
{
    /// <summary>
    ///     Indicates that the API version 0.x and all later minor versions are supported by the configured endpoint.
    /// </summary>
    /// <param name="builder">The endpoint route handler builder.</param>
    /// <param name="minorVersion">The minor version number.</param>
    /// <returns>The same <see cref="RouteHandlerBuilder" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="ArgumentException">The minor version number is not supported.</exception>
    internal static RouteHandlerBuilder IntroducedInVersion0Point(this RouteHandlerBuilder builder, int minorVersion) =>
        minorVersion switch
        {
            1 => builder.HasApiVersion(0, 1).HasApiVersion(0, 2),
            2 => builder.HasApiVersion(0, 2),
            _ => throw new ArgumentException($"Api version 0.{minorVersion} is not supported.")
        };
}
