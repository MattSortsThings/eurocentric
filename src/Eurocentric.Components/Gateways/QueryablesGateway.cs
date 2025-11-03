using System.Linq.Expressions;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Analytics.Queryables;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.Gateways;

internal sealed class QueryablesGateway(AppDbContext dbContext) : IQueryablesGateway
{
    private static readonly Expression<Func<ContestBroadcast, QueryableBroadcast>> MapToQueryableBroadcast =
        contestBroadcast => new QueryableBroadcast
        {
            BroadcastDate = contestBroadcast.Broadcast.BroadcastDate.Value,
            ContestYear = contestBroadcast.Contest.ContestYear.Value,
            CityName = contestBroadcast.Contest.CityName.Value,
            ContestStage = contestBroadcast.Broadcast.ContestStage,
            Competitors = contestBroadcast.Broadcast.Competitors.Count,
            Juries = contestBroadcast.Broadcast.Juries.Count,
            Televotes = contestBroadcast.Broadcast.Televotes.Count,
        };

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

    public async Task<QueryableBroadcast[]> GetQueryableBroadcastsAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Contest> queryableContests = dbContext.Contests.AsNoTracking().Where(contest => contest.Queryable);

        IQueryable<ContestBroadcast> queryableContestsBroadcasts = dbContext
            .Broadcasts.AsNoTracking()
            .Join(
                queryableContests,
                broadcast => broadcast.ParentContestId,
                contest => contest.Id,
                (broadcast, contest) => new ContestBroadcast { Contest = contest, Broadcast = broadcast }
            );

        return await queryableContestsBroadcasts
            .OrderBy(cb => cb.Broadcast.BroadcastDate)
            .Select(MapToQueryableBroadcast)
            .ToArrayAsync(cancellationToken);
    }

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

    private sealed record ContestBroadcast
    {
        public Contest Contest { get; init; } = null!;

        public Broadcast Broadcast { get; init; } = null!;
    }
}
