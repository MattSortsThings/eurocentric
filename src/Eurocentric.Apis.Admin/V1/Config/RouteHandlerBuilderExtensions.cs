using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Apis.Admin.V1.Config;

internal static class RouteHandlerBuilderExtensions
{
    internal static RouteHandlerBuilder AddedInVersion1Point0(this RouteHandlerBuilder builder)
    {
        builder.HasApiVersion(1, 0);

        return builder;
    }
}
