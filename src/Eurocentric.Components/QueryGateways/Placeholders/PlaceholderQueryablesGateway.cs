using System.Linq.Expressions;
using Eurocentric.Components.DataAccess.EFCore;
using Eurocentric.Domain.Aggregates.Placeholders;
using Eurocentric.Domain.Queries.Placeholders;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.QueryGateways.Placeholders;

internal sealed class PlaceholderQueryablesGateway(AppDbContext dbContext) : IQueryablesGateway
{
    private static readonly Expression<Func<Contest, QueryableContest>> MapToQueryableContest =
        contest => new QueryableContest
        {
            ContestYear = contest.ContestYear,
            CityName = contest.CityName,
            Participants = contest.Participants.Count,
            SemiFinalVotingRules = contest.SemiFinalVotingRules,
            GrandFinalVotingRules = contest.GrandFinalVotingRules,
            UsesGlobalTelevote = contest.GlobalTelevote != null,
        };

    private static readonly Expression<Func<Country, QueryableCountry>> MapToQueryableCountry =
        country => new QueryableCountry { CountryCode = country.CountryCode, CountryName = country.CountryName };

    public async Task<List<QueryableContest>> GetQueryableContestsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Contest>()
            .AsNoTracking()
            .Where(contest => contest.Queryable)
            .OrderBy(contest => contest.ContestYear)
            .Select(MapToQueryableContest)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<QueryableCountry>> GetQueryableCountriesAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Guid> contestIdsQuery = dbContext
            .Set<Contest>()
            .AsNoTracking()
            .Where(contest => contest.Queryable)
            .Select(contest => contest.Id);

        IQueryable<QueryableCountry> countriesQuery = dbContext
            .Set<Country>()
            .AsNoTracking()
            .Where(country => country.ContestRoles.Select(role => role.ContestId).Intersect(contestIdsQuery).Any())
            .OrderBy(country => country.CountryCode)
            .Select(MapToQueryableCountry);

        return await countriesQuery.ToListAsync(cancellationToken);
    }
}
