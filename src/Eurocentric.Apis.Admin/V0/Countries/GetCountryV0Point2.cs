using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V0.Common.Configuration;
using Eurocentric.Apis.Admin.V0.Common.Dtos.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Placeholders;
using Eurocentric.Domain.Errors;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using CountryDto = Eurocentric.Apis.Admin.V0.Common.Dtos.Countries.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Countries;

internal static class GetCountryV0Point2
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "countryId")] Guid countryId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<QueryResult, DomainError> result = await bus.Send(
            new Query(countryId),
            cancellationToken: cancellationToken
        );

        return result.IsSuccess ? MapToOkResponse(result.Value) : TypedResults.InternalServerError("Request failed");
    }

    private static Ok<GetCountryResponseBody> MapToOkResponse(in QueryResult result)
    {
        GetCountryResponseBody responseBody = new(result.Country);

        return TypedResults.Ok(responseBody);
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("v0.2/countries/{countryId:guid}", ExecuteAsync)
                .WithName("AdminApi.V0.GetCountryV0Point2")
                .WithSummary("Get a country")
                .WithTags(EndpointTags.CountriesAdmin)
                .Produces<GetCountryResponseBody>()
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal readonly record struct QueryResult(CountryDto Country);

    internal sealed record Query(Guid CountryId) : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "QueryHandler")]
    internal sealed class QueryHandler(ICountryReadRepository repository) : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Guid countryId = query.CountryId;

            return await repository
                .GetUntrackedAsync(countryId, cancellationToken)
                .ToResult(() => CountryErrors.CountryNotFound(countryId))
                .Map(CountryMapper.MapToDto)
                .Map(country => new QueryResult(country));
        }
    }
}
