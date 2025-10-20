using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Apis.Public.V0.Config;

internal static class RouteHandlerBuilderExtensions
{
    internal static RouteHandlerBuilder AddedInVersion0Point1(this RouteHandlerBuilder builder)
    {
        builder.HasApiVersion(0, 1).HasApiVersion(0, 2);

        return builder;
    }

    internal static RouteHandlerBuilder AddedInVersion0Point2(this RouteHandlerBuilder builder)
    {
        builder.HasApiVersion(0, 2);

        return builder;
    }
}
