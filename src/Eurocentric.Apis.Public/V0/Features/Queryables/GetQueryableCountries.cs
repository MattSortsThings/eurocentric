using ErrorOr;
using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using QueryableCountry = Eurocentric.Domain.V0.Views.QueryableCountry;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

internal static class GetQueryableCountriesV0Point1
{
    internal static IEndpointRouteBuilder MapGetQueryableCountriesV0Point1(
        this IEndpointRouteBuilder builder
    )
    {
        builder
            .MapGet("v0.1/queryables/countries", ExecuteAsync)
            .WithName("PublicApi.V0.Queryables.GetQueryableCountriesV0Point1")
            .WithTags(V0Group.Queryables.Tag)
            .Produces<GetQueryableCountriesResponse>();

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default
    )
    {
        ErrorOr<Result> errorsOrResult = await bus.Send(
            new Query(),
            cancellationToken: cancellationToken
        );

        return MapToOk(errorsOrResult.Value);
    }

    private static Ok<GetQueryableCountriesResponse> MapToOk(in Result result)
    {
        GetQueryableCountriesResponse response = new(
            result.QueryableCountries.Select(country => country.ToDto()).ToArray()
        );

        return TypedResults.Ok(response);
    }

    internal readonly record struct Result(QueryableCountry[] QueryableCountries);

    internal sealed record Query : IQuery<Result>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Result>
    {
        public async Task<ErrorOr<Result>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            QueryableCountry[] queryableCountries = await dbContext
                .QueryableCountries.AsNoTracking()
                .OrderBy(country => country.CountryCode)
                .ToArrayAsync(cancellationToken);

            return new Result(queryableCountries);
        }
    }
}
