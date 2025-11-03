using System.Linq.Expressions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Analytics.Queryables;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Gateways;

internal sealed class QueryablesGateway(AppDbContext dbContext) : IQueryablesGateway
{
    private static readonly Expression<Func<Contest, QueryableContest>> MapToQueryableContest =
        contest => new QueryableContest
        {
            ContestYear = contest.ContestYear.Value,
            CityName = contest.CityName.Value,
            Participants = contest.Participants.Count,
            UsesRestOfWorldTelevote = contest.GlobalTelevote != null,
        };

    private static readonly Expression<Func<Country, QueryableCountry>> MapToQueryableCountry =
        country => new QueryableCountry
        {
            CountryCode = country.CountryCode.Value,
            CountryName = country.CountryName.Value,
        };

    public async Task<QueryableContest[]> GetQueryableContestsAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Contest> queryableContests = dbContext.Contests.AsNoTracking().Where(contest => contest.Queryable);

        return await queryableContests
            .OrderBy(contest => contest.ContestYear)
            .Select(MapToQueryableContest)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<QueryableCountry[]> GetQueryableCountriesAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<ContestId> queryableContestIds = dbContext
            .Contests.AsNoTracking()
            .Where(contest => contest.Queryable)
            .Select(contest => contest.Id);

        IQueryable<Country> queryableCountries = dbContext
            .Countries.AsNoTracking()
            .Where(country => country.ContestRoles.Any(role => queryableContestIds.Contains(role.ContestId)));

        return await queryableCountries
            .OrderBy(country => country.CountryCode)
            .Select(MapToQueryableCountry)
            .ToArrayAsync(cancellationToken);
    }
}
