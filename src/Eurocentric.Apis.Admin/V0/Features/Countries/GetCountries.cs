using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Config;
using Eurocentric.Apis.Admin.V0.Dtos.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Aggregates.Countries;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using CountryAggregate = Eurocentric.Domain.V0.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Dtos.Countries.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

internal static class GetCountries
{
    private static Ok<GetCountriesResponse> MapToOk(CountryAggregate[] countries)
    {
        CountryDto[] countryDtos = countries.Select(country => country.ToDto()).ToArray();

        return TypedResults.Ok(new GetCountriesResponse(countryDtos));
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        Result<CountryAggregate[], IDomainError> result = await bus.Send(new Query(), cancellationToken: ct);

        return result.IsSuccess
            ? MapToOk(result.GetValueOrDefault())
            : throw new InvalidOperationException("Query failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("countries", ExecuteAsync)
                .WithName("AdminApi.V0.GetCountries")
                .AddedInVersion0Point1()
                .WithSummary("Get all countries")
                .WithDescription("Retrieves all the countries in the system, ordered by country code.")
                .WithTags(EndpointConstants.Tags.Countries)
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
