using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V1.Dtos.Countries.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

internal static class GetCountry
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(countryId.ToQuery(), MapToOk, ct);

    private static Query ToQuery(this Guid countryId) => new(CountryId.FromValue(countryId));

    private static Ok<GetCountryResponse> MapToOk(CountryAggregate aggregate)
    {
        CountryDto countryDto = aggregate.ToDto();

        return TypedResults.Ok(new GetCountryResponse(countryDto));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("countries/{countryId:guid}", ExecuteAsync)
                .WithName(V1EndpointNames.Countries.GetCountry)
                .AddedInVersion1Point0()
                .WithSummary("Get a country")
                .WithDescription("Retrieves the requested country.")
                .WithTags(V1Tags.Countries)
                .Produces<GetCountryResponse>()
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal sealed record Query(CountryId CountryId) : IQuery<CountryAggregate>;

    [UsedImplicitly]
    internal sealed class QueryHandler(ICountryReadRepository readRepository) : IQueryHandler<Query, CountryAggregate>
    {
        public async Task<Result<CountryAggregate, IDomainError>> OnHandle(Query query, CancellationToken ct) =>
            await readRepository.GetByIdAsync(query.CountryId, ct);
    }
}
