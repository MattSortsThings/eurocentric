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
using CountryAggregate = Eurocentric.Domain.Aggregates.Placeholders.Country;
using CountryDto = Eurocentric.Apis.Admin.V0.Common.Dtos.Countries.Country;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V0.Countries;

internal static class GetCountriesV0Point2
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        Result<QueryResult, DomainError> result = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return result.IsSuccess ? MapToOkResponse(result.Value) : TypedResults.InternalServerError("Request failed");
    }

    private static Ok<GetCountriesResponseBody> MapToOkResponse(QueryResult result)
    {
        GetCountriesResponseBody responseBody = new(result.Countries);

        return TypedResults.Ok(responseBody);
    }

    internal sealed class Endpoint : IEndpointMapper
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            builder
                .MapGet("v0.2/countries", ExecuteAsync)
                .WithName("AdminApi.V0.GetCountriesV0Point2")
                .WithSummary("Get all countries")
                .WithTags(EndpointTags.CountriesAdmin)
                .Produces<GetCountriesResponseBody>();
        }
    }

    internal readonly record struct QueryResult(CountryDto[] Countries);

    internal sealed record Query : IQuery<QueryResult>;

    [UsedImplicitly(Reason = "QueryHandler")]
    internal sealed class QueryHandler(ICountryReadRepository repository) : IQueryHandler<Query, QueryResult>
    {
        public async Task<Result<QueryResult, DomainError>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            List<CountryAggregate> countries = await repository.GetAllUntrackedAsync(
                country => country.CountryCode,
                cancellationToken
            );

            return new QueryResult(countries.Select(CountryMapper.MapToDto).ToArray());
        }
    }
}
