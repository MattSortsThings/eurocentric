using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Versioning;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableContestStages;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;
using Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableVotingMethods;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V0.Queryables;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the "Queryables" tagged endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    internal static void MapQueryablesEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder endpointGroup = builder.MapGroup("queryables")
            .WithTags(EndpointNames.Queryables.Tag)
            .WithDescription("Endpoints for accessing lists of queryable data.");

        endpointGroup.MapGet("contest-stages", GetQueryableContestStagesFeature.ExecuteAsync)
            .WithName(EndpointNames.Queryables.GetQueryableContestStages)
            .WithSummary("Get queryable contest stages")
            .WithDescription("Retrieves all the `QueryableContestStage` enum values.")
            .IntroducedInVersion0Point(1)
            .Produces<GetQueryableContestStagesResponse>();

        endpointGroup.MapGet("countries", GetQueryableCountriesFeature.ExecuteAsync)
            .WithName(EndpointNames.Queryables.GetQueryableCountries)
            .WithSummary("Get queryable countries")
            .WithDescription("Retrieves all the queryable countries, ordered by country code.")
            .IntroducedInVersion0Point(1)
            .Produces<GetQueryableCountriesResponse>();

        endpointGroup.MapGet("voting-methods", GetQueryableVotingMethodsFeature.ExecuteAsync)
            .WithName(EndpointNames.Queryables.GetQueryableVotingMethods)
            .WithSummary("Get queryable voting methods")
            .WithDescription("Retrieves all the `QueryableVotingMethod` enum values.")
            .IntroducedInVersion0Point(1)
            .Produces<GetQueryableVotingMethodsResponse>();
    }
}
