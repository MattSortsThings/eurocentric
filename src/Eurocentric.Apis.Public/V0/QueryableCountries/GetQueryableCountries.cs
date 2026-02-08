using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Common.Constants;
using Eurocentric.Components.Endpoints;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Placeholders.Analytics.Queryables;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;
using QueryableCountryItem = Eurocentric.Domain.Placeholders.Analytics.Queryables.QueryableCountry;
using QueryableCountryModel = Eurocentric.Apis.Public.V0.Common.Models.Queryables.QueryableCountry;

namespace Eurocentric.Apis.Public.V0.QueryableCountries;

[Mapper]
internal static partial class GetQueryableCountriesV0Point1
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<QueryResult, DomainError> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.Match(MapToOk, MapToInternalServerError);
    }

    private static IResult MapToOk(QueryResult result) => TypedResults.Ok(result.ToResponseBody());

    private static IResult MapToInternalServerError(DomainError error) => TypedResults.InternalServerError(error.Title);

    [MapPropertyFromSource(nameof(QueryResult.QueryableCountries))]
    private static partial QueryResult ToQueryResult(this List<QueryableCountryItem> items);

    private static partial GetQueryableCountriesResponseBody ToResponseBody(this QueryResult result);

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("v0.1/queryable-countries", ExecuteAsync)
                .WithName(EndpointNames.GetQueryableCountriesV0Point1)
                .WithDisplayName(EndpointDisplayNames.GetQueryableCountriesV0Point1)
                .WithSummary("Get queryable countries")
                .WithDescription("Lists all queryable countries, ordered by country code.")
                .WithTags(EndpointTags.QueryableCountries)
                .Produces<GetQueryableCountriesResponseBody>();
        }
    }

    internal readonly record struct QueryResult(QueryableCountryModel[] QueryableCountries);

    internal sealed record Query : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "messaging")]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            List<QueryableCountryItem> items = await gateway.GetQueryableCountriesAsync(cancellationToken);

            return items.ToQueryResult();
        }
    }
}
