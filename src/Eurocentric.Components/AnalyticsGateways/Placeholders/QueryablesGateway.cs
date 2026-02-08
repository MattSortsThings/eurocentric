using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.Placeholders.Aggregates;
using Eurocentric.Domain.Placeholders.Analytics.Queryables;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Components.AnalyticsGateways.Placeholders;

internal sealed class QueryablesGateway(AppDbContext dbContext) : IQueryablesGateway
{
    public async Task<List<QueryableBroadcast>> GetQueryableBroadcastsAsync(
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Contest> queryableContests = dbContext
            .Set<Contest>()
            .AsNoTracking()
            .Where(contest => contest.Queryable);

        IQueryable<Country> countries = dbContext.Set<Country>().AsNoTracking();

        IQueryable<Broadcast> queryableBroadcasts = dbContext
            .Set<Broadcast>()
            .AsNoTracking()
            .Join(
                queryableContests.SelectMany(contest => contest.BroadcastMemos),
                broadcast => broadcast.Id,
                broadcastMemo => broadcastMemo.BroadcastId,
                (broadcast, _) => broadcast
            );

        IQueryable<QueryableBroadcast> finalQuery = queryableBroadcasts
            .OrderBy(broadcast => broadcast.BroadcastDate)
            .Select(broadcast => new QueryableBroadcast
            {
                BroadcastDate = broadcast.BroadcastDate,
                ContestStage = broadcast.ContestStage,
                VotingFormat = broadcast.VotingFormat,
                CompetingCountryCodes = broadcast
                    .Competitors.Join(
                        countries,
                        competitor => competitor.CompetingCountryId,
                        country => country.Id,
                        (_, country) => country.CountryCode
                    )
                    .OrderBy(countryCode => countryCode)
                    .ToList(),
                VotingCountryCodes = broadcast
                    .Competitors.SelectMany(competitor => competitor.PointsAwards)
                    .Join(
                        countries,
                        pointsAward => pointsAward.VotingCountryId,
                        country => country.Id,
                        (_, country) => country.CountryCode
                    )
                    .Distinct()
                    .OrderBy(countryCode => countryCode)
                    .ToList(),
            });

        return await finalQuery.ToListAsync(cancellationToken);
    }

    public async Task<List<QueryableContest>> GetQueryableContestsAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Country> countries = dbContext.Set<Country>().AsNoTracking();

        IQueryable<Contest> queryableContests = dbContext
            .Set<Contest>()
            .AsNoTracking()
            .Where(contest => contest.Queryable);

        IQueryable<QueryableContest> finalQuery = queryableContests
            .OrderBy(contest => contest.ContestYear)
            .Select(contest => new QueryableContest
            {
                ContestYear = contest.ContestYear,
                CityName = contest.CityName,
                SemiFinalVotingFormat = contest.SemiFinalVotingFormat,
                GrandFinalVotingFormat = contest.GrandFinalVotingFormat,
                GlobalTelevoteCountryCode =
                    contest.GlobalTelevote != null
                        ? countries.First(country => country.Id == contest.GlobalTelevote.VotingCountryId).CountryCode
                        : null,
                ParticipatingCountryCodes = contest
                    .Participants.Join(
                        countries,
                        participant => participant.ParticipatingCountryId,
                        country => country.Id,
                        (_, country) => country.CountryCode
                    )
                    .OrderBy(countryCode => countryCode)
                    .ToList(),
            });

        return await finalQuery.ToListAsync(cancellationToken);
    }

    public async Task<List<QueryableCountry>> GetQueryableCountriesAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Contest> queryableContests = dbContext
            .Set<Contest>()
            .AsNoTracking()
            .Where(contest => contest.Queryable);

        IOrderedQueryable<QueryableCountry> finalQuery = dbContext
            .Set<Country>()
            .AsNoTracking()
            .Select(country => new QueryableCountry
            {
                CountryCode = country.CountryCode,
                CountryName = country.CountryName,
                ActiveContestYears = country
                    .ContestIds.Join(
                        queryableContests,
                        contestId => contestId,
                        contest => contest.Id,
                        (_, contest) => contest.ContestYear
                    )
                    .OrderBy(contestYear => contestYear)
                    .ToList(),
            })
            .Where(country => country.ActiveContestYears.Any())
            .OrderBy(country => country.CountryCode);

        return await finalQuery.ToListAsync(cancellationToken);
    }
}
