using System.Linq.Expressions;
using ErrorOr;
using Eurocentric.Apis.Public.V0.Constants;
using Eurocentric.Apis.Public.V0.Dtos.Queryables;
using Eurocentric.Apis.Public.V0.Versioning;
using Eurocentric.Domain.V0Entities;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.Infrastructure.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Apis.Public.V0.Features.Queryables;

public static class GetQueryableCountries
{
    private static readonly Expression<Func<Country, QueryableCountry>> MapCountryToQueryableCountry = country =>
        new QueryableCountry { CountryCode = country.CountryCode, CountryName = country.CountryName };

    internal static IEndpointRouteBuilder MapGetQueryableCountries(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/queryables/countries", ExecuteAsync)
            .WithName(V0Group.Queryables.Endpoints.GetQueryableCountries)
            .IntroducedInV0Point1()
            .WithTags(V0Group.Queryables.Tag);

        return builder;
    }

    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<Response> errorsOrResponse = await bus.Send(new Query(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    public sealed record Response(QueryableCountry[] QueryableCountries);

    internal sealed record Query : IQuery<Response>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, Response>
    {
        public async Task<ErrorOr<Response>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            IQueryable<Guid> contestIds = dbContext.V0Contests.AsNoTracking()
                .Where(contest => contest.Queryable)
                .Select(contest => contest.Id);

            IQueryable<QueryableCountry> countries = dbContext.V0Countries.AsNoTracking()
                .Where(country => country.ContestRoles.Any(role => contestIds.Contains(role.ContestId)))
                .OrderBy(country => country.CountryCode)
                .Select(MapCountryToQueryableCountry);

            QueryableCountry[] queryableCountries = await countries.ToArrayAsync(cancellationToken);

            return new Response(queryableCountries);
        }
    }
}
