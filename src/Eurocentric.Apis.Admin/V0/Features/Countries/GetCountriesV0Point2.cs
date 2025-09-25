using ErrorOr;
using Eurocentric.Apis.Admin.V0.Constants;
using Eurocentric.Apis.Admin.V0.Contracts.Countries;
using Eurocentric.Apis.Admin.V0.Contracts.Mapping;
using Eurocentric.Domain.V0.Aggregates.Countries;
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

namespace Eurocentric.Apis.Admin.V0.Features.Countries;

internal static class GetCountriesV0Point2
{
    internal static IEndpointRouteBuilder MapGetCountriesV0Point2(
        this IEndpointRouteBuilder builder
    )
    {
        builder
            .MapGet("v0.2/countries", ExecuteAsync)
            .WithName("AdminApi.V0.Countries.GetCountriesV0Point2")
            .WithTags(V0Group.Countries.Tag)
            .Produces<GetCountriesResponse>();

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

        return TypedResults.Ok(errorsOrResult.Value);
    }

    private static Ok<GetCountriesResponse> MapToOk(in Result result)
    {
        GetCountriesResponse response = new(
            result.Countries.Select(country => country.ToCountryDto()).ToArray()
        );

        return TypedResults.Ok(response);
    }

    internal readonly record struct Result(Country[] Countries);

    internal sealed record Query : IQuery<Result>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Result>
    {
        public async Task<ErrorOr<Result>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Country[] countries = await dbContext
                .Countries.AsNoTracking()
                .OrderBy(country => country.CountryCode)
                .ToArrayAsync(cancellationToken);

            return new Result(countries);
        }
    }
}
