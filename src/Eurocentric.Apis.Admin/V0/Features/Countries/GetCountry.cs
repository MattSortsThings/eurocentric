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

internal static class GetCountry
{
    private static Ok<GetCountryResponse> MapToOk(CountryAggregate country)
    {
        CountryDto countryDto = country.ToDto();

        return TypedResults.Ok(new GetCountryResponse(countryDto));
    }

    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        Result<CountryAggregate, IDomainError> result = await bus.Send(new Query(countryId), cancellationToken: ct);

        return result.IsSuccess
            ? MapToOk(result.GetValueOrDefault())
            : throw new InvalidOperationException("Query failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("countries/{countryId:guid}", ExecuteAsync)
                .WithName("AdminApi.V0.GetCountry")
                .AddedInVersion0Point1()
                .WithSummary("Get a country")
                .WithDescription("Retrieves a single country in the system, specified by its ID.")
                .WithTags(EndpointConstants.Tags.Countries)
                .Produces<GetCountryResponse>()
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal sealed record Query(Guid CountryId) : IQuery<CountryAggregate>;

    [UsedImplicitly]
    internal sealed class QueryHandler(ICountryReadRepository readRepository) : IQueryHandler<Query, CountryAggregate>
    {
        public async Task<Result<CountryAggregate, IDomainError>> OnHandle(Query query, CancellationToken ct) =>
            await readRepository.GetByIdAsync(query.CountryId, ct);
    }
}
