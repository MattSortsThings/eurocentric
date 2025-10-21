using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
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

internal static class GetQueryableCountries
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetQueryableCountriesResponse> MapToOk(QueryableCountryRecord[] records)
    {
        QueryableCountryDto[] dtos = records.Select(country => country.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableCountriesResponse(dtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("queryables/countries", ExecuteAsync)
                .WithName("PublicApi.V0.GetQueryableCountries")
                .AddedInVersion0Point1()
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
