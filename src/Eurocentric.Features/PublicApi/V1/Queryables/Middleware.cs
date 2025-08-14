using Eurocentric.Features.PublicApi.V1.Common.Constants;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableBroadcasts;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContests;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContestStages;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableCountries;
using Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableVotingMethods;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V1.Queryables;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Queryables".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapQueryablesEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("queryables")
            .WithTags(EndpointNames.Tags.Queryables);

        group.MapGet("broadcasts", GetQueryableBroadcastsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Queryables.GetQueryableBroadcasts)
            .WithSummary("Get queryable broadcasts")
            .WithDescription("Retrieves a list of all the queryable broadcasts, in broadcast date order.")
            .HasApiVersion(1, 0)
            .Produces<GetQueryableBroadcastsResponse>();

        group.MapGet("contest-stages", GetQueryableContestStagesFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Queryables.GetQueryableContestStages)
            .WithSummary("Get queryable contest stages")
            .WithDescription("Retrieves an ordered list of all QueryableContestStage enum values.")
            .HasApiVersion(1, 0)
            .Produces<GetQueryableContestStagesResponse>();

        group.MapGet("contests", GetQueryableContestsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Queryables.GetQueryableContests)
            .WithSummary("Get queryable contests")
            .WithDescription("Retrieves a list of all the queryable contests, in contest year order.")
            .HasApiVersion(1, 0)
            .Produces<GetQueryableContestsResponse>();

        group.MapGet("countries", GetQueryableCountriesFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Queryables.GetQueryableCountries)
            .WithSummary("Get queryable countries")
            .WithDescription("Retrieves a list of all the queryable countries, in country code order.")
            .HasApiVersion(1, 0)
            .Produces<GetQueryableCountriesResponse>();

        group.MapGet("voting-methods", GetQueryableVotingMethodsFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Queryables.GetQueryableVotingMethods)
            .WithSummary("Get queryable voting methods")
            .WithDescription("Retrieves an ordered list of all QueryableVotingMethod enum values.")
            .HasApiVersion(1, 0)
            .Produces<GetQueryableVotingMethodsResponse>();
    }
}
