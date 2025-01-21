using Eurocentric.PublicApi.V1.VotingCountryRankings.GetVotingCountryPointsShareRankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.PublicApi;

public static class PublicApiPlaceholder
{
    public static IEndpointRouteBuilder MapPublicApiPlaceholderEndpoint(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.MapGroup("public/api/v1.0")
            .AllowAnonymous();

        api.MapGetVotingCountryPointsShare();

        return app;
    }
}
