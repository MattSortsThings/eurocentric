using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V1.Config;
using Eurocentric.Apis.Public.V1.Dtos.Queryables;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Analytics.Queryables;
using Eurocentric.Domain.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;
using QueryableCountryDto = Eurocentric.Apis.Public.V1.Dtos.Queryables.QueryableCountry;
using QueryableCountryRow = Eurocentric.Domain.Analytics.Queryables.QueryableCountry;

namespace Eurocentric.Apis.Public.V1.Features.Queryables;

internal static class GetQueryableCountries
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetQueryableCountriesResponse> MapToOk(QueryableCountryRow[] rows)
    {
        QueryableCountryDto[] dtos = rows.Select(qc => qc.ToDto()).ToArray();

        return TypedResults.Ok(new GetQueryableCountriesResponse(dtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("queryables/countries", ExecuteAsync)
                .WithName(V1EndpointNames.Queryables.GetQueryableCountries)
                .AddedInVersion1Point0()
                .WithSummary("Get all queryable countries")
                .WithDescription("Retrieves a list of all the queryable countries, ordered by country code.")
                .WithTags(V1Tags.Queryables)
                .Produces<GetQueryableCountriesResponse>();
        }
    }

    internal sealed record Query : IQuery<QueryableCountryRow[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryableCountryRow[]>
    {
        public async Task<Result<QueryableCountryRow[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await gateway.GetQueryableCountriesAsync(ct);
    }
}
