using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
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

internal static class GetCountries
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetCountriesResponse> MapToOk(CountryAggregate[] aggregates)
    {
        CountryDto[] countryDtos = aggregates.Select(country => country.ToDto()).ToArray();

        return TypedResults.Ok(new GetCountriesResponse(countryDtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("countries", ExecuteAsync)
                .WithName(V1EndpointNames.Countries.GetCountries)
                .AddedInVersion1Point0()
                .WithSummary("Get all countries")
                .WithDescription("Retrieves all the countries in the system, ordered by country code.")
                .WithTags(V1Tags.Countries)
                .Produces<GetCountriesResponse>();
        }
    }

    internal sealed record Query : IQuery<CountryAggregate[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(ICountryReadRepository readRepository) : IQueryHandler<Query, CountryAggregate[]>
    {
        public async Task<Result<CountryAggregate[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await readRepository.GetAllAsync(ct);
    }
}
