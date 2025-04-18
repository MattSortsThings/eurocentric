using Eurocentric.Features.AdminApi.V0.Contests.CreateContest;
using Eurocentric.Features.AdminApi.V0.Contests.GetContest;
using Eurocentric.Features.AdminApi.V0.Contests.GetContests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi;

internal static class EndpointMapping
{
    internal static void MapAdminApiEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.MapGroup("admin/api").AllowAnonymous();

        apiGroup.MapGetContestsV0Point1();
        apiGroup.MapGetContestsV0Point2();
        apiGroup.MapGetContestV0Point1();
        apiGroup.MapGetContestV0Point2();
        apiGroup.MapCreateContestV0Point2();
    }
}
