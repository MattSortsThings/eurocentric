using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Queries.Queryables;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;
using QueryableCountryDto = Eurocentric.Apis.Public.V0.Dtos.Queryables.QueryableCountry;
using QueryableCountryRecord = Eurocentric.Domain.V0.Queries.Queryables.QueryableCountry;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

internal static class GetQueryableCountriesV0Point1
{
    private static Ok<GetQueryableCountriesResponse> MapToOk(QueryableCountryRecord[] records)
    {
        QueryableCountryDto[] dtos = records.Select(country => country.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableCountriesResponse(dtos));
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        Result<QueryableCountryRecord[], IDomainError> result = await bus.Send(new Query(), cancellationToken: ct);

        return result.IsSuccess
            ? MapToOk(result.GetValueOrDefault())
            : throw new InvalidOperationException("Query failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("v0.1/queryables/countries", ExecuteAsync)
                .WithName("PublicApi.V0.1.GetQueryableCountries")
                .WithSummary("Get all queryable countries")
                .WithDescription("Retrieves all the queryable countries, ordered by country code.")
                .WithTags(EndpointConstants.Tags.Queryables)
                .Produces<GetQueryableCountriesResponse>();
        }
    }

    internal sealed record Query : IQuery<QueryableCountryRecord[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryableCountryRecord[]>
    {
        public async Task<Result<QueryableCountryRecord[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await gateway.GetQueryableCountriesAsync(ct);
    }
}

internal static class GetQueryableCountriesV0Point2
{
    private static Ok<GetQueryableCountriesResponse> MapToOk(QueryableCountryRecord[] records)
    {
        QueryableCountryDto[] dtos = records.Select(country => country.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableCountriesResponse(dtos));
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        Result<QueryableCountryRecord[], IDomainError> result = await bus.Send(new Query(), cancellationToken: ct);

        return result.IsSuccess
            ? MapToOk(result.GetValueOrDefault())
            : throw new InvalidOperationException("Query failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("v0.2/queryables/countries", ExecuteAsync)
                .WithName("PublicApi.V0.2.GetQueryableCountries")
                .WithSummary("Get all queryable countries")
                .WithDescription("Retrieves all the queryable countries, ordered by country code.")
                .WithTags(EndpointConstants.Tags.Queryables)
                .Produces<GetQueryableCountriesResponse>();
        }
    }

    internal sealed record Query : IQuery<QueryableCountryRecord[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryableCountryRecord[]>
    {
        public async Task<Result<QueryableCountryRecord[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await gateway.GetQueryableCountriesAsync(ct);
    }
}
