using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Features.PublicApi.V0.Common.Versioning;

/// <summary>
///     Extension methods to be invoked when configuring endpoints.
/// </summary>
internal static class RouteHandlerBuilderExtensions
{
    /// <summary>
    ///     Indicates that API versions 0.1 and 0.2 are supported by the configured endpoint.
    /// </summary>
    /// <param name="builder">The endpoint route handler builder.</param>
    /// <returns>The same <see cref="RouteHandlerBuilder" /> instance, so that method invocations can be chained.</returns>
    internal static RouteHandlerBuilder IntroducedInVersion0Point1(this RouteHandlerBuilder builder) =>
        builder.HasApiVersion(0, 1).HasApiVersion(0, 2);

    /// <summary>
    ///     Indicates that API version 0.2 is supported by the configured endpoint.
    /// </summary>
    /// <param name="builder">The endpoint route handler builder.</param>
    /// <returns>The same <see cref="RouteHandlerBuilder" /> instance, so that method invocations can be chained.</returns>
    internal static RouteHandlerBuilder IntroducedInVersion0Point2(this RouteHandlerBuilder builder) =>
        builder.HasApiVersion(0, 2);
}
