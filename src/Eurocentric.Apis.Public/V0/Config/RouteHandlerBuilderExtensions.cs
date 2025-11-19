using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Apis.Public.V0.Config;

internal static class RouteHandlerBuilderExtensions
{
    extension(RouteHandlerBuilder builder)
    {
        internal RouteHandlerBuilder AddedInVersion0Point1()
        {
            builder.HasDeprecatedApiVersion(0, 1).HasDeprecatedApiVersion(0, 2);

            return builder;
        }

        internal RouteHandlerBuilder AddedInVersion0Point2()
        {
            builder.HasDeprecatedApiVersion(0, 2);

            return builder;
        }
    }
}
