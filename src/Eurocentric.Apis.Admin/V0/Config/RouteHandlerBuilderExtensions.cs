using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Apis.Admin.V0.Config;

internal static class RouteHandlerBuilderExtensions
{
    internal static RouteHandlerBuilder AddedInVersion0Point1(this RouteHandlerBuilder builder)
    {
        builder.HasDeprecatedApiVersion(0, 1).HasDeprecatedApiVersion(0, 2);

        return builder;
    }
}
