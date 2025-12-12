using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Common.Config;
using Eurocentric.Apis.Admin.V0.Common.Contracts.Countries;
using Eurocentric.Components.Endpoints;
using Eurocentric.Domain.Abstractions.Errors;
using Eurocentric.Domain.Abstractions.Messaging;
using Eurocentric.Domain.Aggregates.V0;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using Country = Eurocentric.Domain.Aggregates.V0.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Countries;

internal static class GetCountries
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<List<Country>, IDomainError> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.IsSuccess ? MapToOk(result.Value) : throw new InvalidOperationException("Request failed.");
    }

    private static Ok<GetCountriesResponse> MapToOk(List<Country> countries) =>
        TypedResults.Ok(countries.ToGetCountriesResponse());

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("v0.1/countries", ExecuteAsync)
                .WithName(EndpointIds.Countries.GetCountries)
                .WithSummary("Get all countries")
                .WithDescription("Retrieves a list of all existing countries, ordered by country code.")
                .WithTags(EndpointTags.Countries)
                .Produces<GetCountriesResponse>();
        }
    }

    internal sealed record Query : IQuery<List<Country>>;

    [UsedImplicitly]
    internal sealed class QueryHandler(ICountryRepository repository) : IQueryHandler<Query, List<Country>>
    {
        public async Task<Result<List<Country>, IDomainError>> OnHandle(Query _, CancellationToken cancellationToken) =>
            await repository.GetAllUntrackedAsync(cancellationToken);
    }
}
