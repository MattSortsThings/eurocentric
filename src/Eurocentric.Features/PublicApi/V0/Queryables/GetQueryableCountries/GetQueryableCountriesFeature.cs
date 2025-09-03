using ErrorOr;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;

internal static class GetQueryableCountriesFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetQueryableCountriesResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetQueryableCountriesResponse>
    {
        public async Task<ErrorOr<GetQueryableCountriesResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            IQueryable<Guid> queryableContestIdQuery = dbContext.V0Contests.AsNoTracking()
                .Where(contest => contest.Completed)
                .Select(contest => contest.Id);

            IQueryable<QueryableCountry> queryableCountryQuery = dbContext.V0Countries.AsNoTracking()
                .Where(country => queryableContestIdQuery.Intersect(country.ParticipatingContestIds).Any())
                .OrderBy(country => country.CountryCode)
                .Select(country => new QueryableCountry
                {
                    CountryCode = country.CountryCode, CountryName = country.CountryName
                });

            QueryableCountry[] countries = await queryableCountryQuery.ToArrayAsync(cancellationToken);

            return new GetQueryableCountriesResponse(countries);
        }
    }
}
