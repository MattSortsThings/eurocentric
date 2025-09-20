using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Apis.Admin.V0.Versioning;

internal static class RouteHandlerBuilderExtensions
{
    internal static RouteHandlerBuilder IntroducedInV0Point1(this RouteHandlerBuilder builder)
    {
        builder.HasApiVersion(0, 1)
            .HasApiVersion(0, 2);

        return builder;
    }

    internal static RouteHandlerBuilder IntroducedInV0Point2(this RouteHandlerBuilder builder)
    {
        builder.HasApiVersion(0, 2);

        return builder;
    }
}
