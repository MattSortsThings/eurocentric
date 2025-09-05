using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Versioning;
using Eurocentric.Features.AdminApi.V1.Contests.GetContest;
using Eurocentric.Features.AdminApi.V1.Contests.GetContests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1.Contests;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the "Contests" tagged endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    internal static void MapContestsEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder endpointGroup = builder.MapGroup("contests")
            .WithTags(Endpoints.Contests.Tag)
            .WithDescription("Operations on the Contest resource.");

        endpointGroup.MapGet("/", GetContestsFeature.ExecuteAsync)
            .WithName(Endpoints.Contests.GetContests)
            .WithSummary("Get all contests")
            .WithDescription("Retrieves all existing contests from the system, ordered by contest year.")
            .IntroducedInVersion1Point0()
            .Produces<GetContestsResponse>();

        endpointGroup.MapGet("/{contestId:guid}", GetContestFeature.ExecuteAsync)
            .WithName(Endpoints.Contests.GetContest)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest from the system.")
            .IntroducedInVersion1Point0()
            .Produces<GetContestResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
