using System.Linq.Expressions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Analytics.Queryables;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Gateways;

internal sealed class QueryablesGateway(AppDbContext dbContext) : IQueryablesGateway
{
    private static readonly Expression<Func<Country, QueryableCountry>> MapToQueryableCountry =
        country => new QueryableCountry
        {
            CountryCode = country.CountryCode.Value,
            CountryName = country.CountryName.Value,
        };

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
