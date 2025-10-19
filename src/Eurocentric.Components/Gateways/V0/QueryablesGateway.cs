using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.V0.Aggregates.Contests;
using Eurocentric.Domain.V0.Aggregates.Countries;
using Eurocentric.Domain.V0.Queries.Queryables;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Gateways.V0;

internal sealed class QueryablesGateway(AppDbContext dbContext) : IQueryablesGateway
{
    public async Task<QueryableBroadcast[]> GetQueryableBroadcastsAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Contest> queryableContests = dbContext.V0Contests.AsNoTracking().Where(contest => contest.Queryable);

        var queryableBroadcastsAndContests = dbContext
            .V0Broadcasts.AsNoTracking()
            .Join(
                queryableContests,
                broadcast => broadcast.ParentContestId,
                contest => contest.Id,
                (broadcast, contest) => new { broadcast, contest }
            )
            .OrderBy(item => item.broadcast.BroadcastDate);

        return await queryableBroadcastsAndContests
            .Select(item => new QueryableBroadcast
            {
                BroadcastDate = item.broadcast.BroadcastDate,
                ContestYear = item.contest.ContestYear,
                CityName = item.contest.CityName,
                Competitors = item.broadcast.Competitors.Count(),
                Juries = item.broadcast.Juries.Count(),
                Televotes = item.broadcast.Televotes.Count(),
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<QueryableCountry[]> GetQueryableCountriesAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Guid> queryableContestIds = dbContext
            .V0Contests.AsNoTracking()
            .Where(contest => contest.Queryable)
            .Select(contest => contest.Id);

        IOrderedQueryable<Country> queryableCountries = dbContext
            .V0Countries.AsNoTracking()
            .Where(country => country.ContestRoles.Any(role => queryableContestIds.Contains(role.ContestId)))
            .OrderBy(country => country.CountryCode);

        return await queryableCountries
            .Select(country => new QueryableCountry
            {
                CountryCode = country.CountryCode,
                CountryName = country.CountryName,
            })
            .ToArrayAsync(cancellationToken);
    }
}
