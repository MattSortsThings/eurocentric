using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Common.Configuration;
using Eurocentric.Apis.Public.V0.Common.Dtos.Countries;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Errors;
using Eurocentric.Domain.Queries.Placeholders;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Public.V0.Countries;

internal static class GetQueryableCountriesV0Point1
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<QueryResult, DomainError> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.IsSuccess ? MapToOkResponse(result.Value) : TypedResults.InternalServerError("Request failed");
    }

    private static Ok<GetQueryableCountriesResponseBody> MapToOkResponse(QueryResult result)
    {
        GetQueryableCountriesResponseBody responseBody = new(result.Countries);

        return TypedResults.Ok(responseBody);
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("v0.1/countries", ExecuteAsync)
                .WithName("PublicApi.V0.GetQueryableCountriesV0Point1")
                .WithSummary("Get queryable countries")
                .WithTags(EndpointTags.Countries)
                .Produces<GetQueryableCountriesResponseBody>();
        }
    }

    internal readonly record struct QueryResult(Country[] Countries);

    internal sealed record Query : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "QueryHandler")]
    internal sealed class QueryHandler(IQueryablesGateway gateway) : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            List<QueryableCountry> countries = await gateway.GetQueryableCountriesAsync(cancellationToken);

            return new QueryResult(countries.Select(QueryableCountryMapper.ToCountryDto).ToArray());
        }
    }
}
