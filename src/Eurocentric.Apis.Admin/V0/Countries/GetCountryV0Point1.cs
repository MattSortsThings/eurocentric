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

internal static class GetCountryV0Point1
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<Country, IDomainError> result = await bus.Send(
            new Query(countryId),
            cancellationToken: cancellationToken
        );

        return result.IsSuccess ? MapToOk(result.Value) : throw new InvalidOperationException("Request failed.");
    }

    private static Ok<GetCountryResponse> MapToOk(Country country) => TypedResults.Ok(country.ToGetCountryResponse());

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("v0.1/countries/{countryId:guid}", ExecuteAsync)
                .WithName(EndpointIds.Countries.GetCountryV0Point1)
                .WithSummary("Get a country")
                .WithDescription("Retrieves the requested country.")
                .WithTags(EndpointTags.Countries)
                .Produces<GetCountryResponse>();
        }
    }

    internal sealed record Query(Guid CountryId) : IQuery<Country>;

    [UsedImplicitly]
    internal sealed class QueryHandler(ICountryReadRepository repository) : IQueryHandler<Query, Country>
    {
        public async Task<Result<Country, IDomainError>> OnHandle(Query query, CancellationToken cancellationToken) =>
            await repository.GetUntrackedAsync(query.CountryId, cancellationToken);
    }
}
