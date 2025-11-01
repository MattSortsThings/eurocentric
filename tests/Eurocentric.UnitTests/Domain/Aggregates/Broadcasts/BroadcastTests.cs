using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.TestUtils;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Broadcasts;

public sealed partial class BroadcastTests : UnitTest
{
    private static readonly BroadcastId DefaultBroadcastId = BroadcastId.FromValue(
        Guid.Parse("94f10214-372a-4c67-9328-8309341aadb9")
    );

    private static readonly ContestId DefaultContestId = ContestId.FromValue(
        Guid.Parse("fe901f2e-35ab-4c3a-a828-8d0fd019a182")
    );

    private static readonly BroadcastDate DefaultBroadcastDate = BroadcastDate
        .FromValue(new DateOnly(2016, 01, 01))
        .GetValueOrDefault();

    private static readonly ContestYear DefaultContestYear = ContestYear.FromValue(2016).GetValueOrDefault();
    private static readonly CityName DefaultCityName = CityName.FromValue("CityName").GetValueOrDefault();
    private static readonly ActName DefaultActName = ActName.FromValue("ActName").GetValueOrDefault();
    private static readonly SongTitle DefaultSongTitle = SongTitle.FromValue("SongTitle").GetValueOrDefault();

    private static Broadcast CreateTelevoteOnlyBroadcast(
        CountryId[] extraVotingCountryIds = null!,
        CountryId[] competingCountryIds = null!
    )
    {
        if (extraVotingCountryIds.Length < 1)
        {
            throw new ArgumentException("Must provide at least 1 extra voting country ID.");
        }

        if (competingCountryIds.Length < 3)
        {
            throw new ArgumentException("Must provide at least 3 competing country ID.");
        }

        IEnumerable<CountryId> semiFinal1CountryIds = extraVotingCountryIds.SkipLast(1).Concat(competingCountryIds);
        IEnumerable<CountryId> semiFinal2CountryIds = CountryIds.Generate(3);

        IContestBuilder builder = LiverpoolRulesContest
            .Create()
            .WithContestYear(DefaultContestYear)
            .WithCityName(DefaultCityName)
            .WithGlobalTelevote(extraVotingCountryIds.Last());

        foreach (CountryId countryId in semiFinal1CountryIds)
        {
            builder.AddSemiFinal1Participant(countryId, DefaultActName, DefaultSongTitle);
        }

        foreach (CountryId countryId in semiFinal2CountryIds)
        {
            builder.AddSemiFinal2Participant(countryId, DefaultActName, DefaultSongTitle);
        }

        Contest contest = builder.Build(() => DefaultContestId).GetValueOrDefault();

        return contest
            .CreateSemiFinal1Broadcast()
            .WithBroadcastDate(DefaultBroadcastDate)
            .WithCompetingCountries(competingCountryIds)
            .Build(() => DefaultBroadcastId)
            .GetValueOrDefault();
    }

    private static Broadcast CreateJuryAndTelevoteBroadcast(
        CountryId[] extraVotingCountryIds = null!,
        CountryId[] competingCountryIds = null!
    )
    {
        if (competingCountryIds.Length < 3)
        {
            throw new ArgumentException("Must provide at least 3 competing country ID.");
        }

        IEnumerable<CountryId> semiFinal1CountryIds = extraVotingCountryIds.Concat(competingCountryIds);
        IEnumerable<CountryId> semiFinal2CountryIds = CountryIds.Generate(3);

        IContestBuilder builder = StockholmRulesContest
            .Create()
            .WithContestYear(DefaultContestYear)
            .WithCityName(DefaultCityName);

        foreach (CountryId countryId in semiFinal1CountryIds)
        {
            builder.AddSemiFinal1Participant(countryId, DefaultActName, DefaultSongTitle);
        }

        foreach (CountryId countryId in semiFinal2CountryIds)
        {
            builder.AddSemiFinal2Participant(countryId, DefaultActName, DefaultSongTitle);
        }

        Contest contest = builder.Build(() => DefaultContestId).GetValueOrDefault();

        return contest
            .CreateSemiFinal1Broadcast()
            .WithBroadcastDate(DefaultBroadcastDate)
            .WithCompetingCountries(competingCountryIds)
            .Build(() => DefaultBroadcastId)
            .GetValueOrDefault();
    }

    private static void AwardTelevotePoints(Broadcast broadcast, params IAwardParams[] parameters)
    {
        foreach (IAwardParams p in parameters)
        {
            broadcast.AwardTelevotePoints(p);
        }
    }

    private sealed class AwardParams : IAwardParams
    {
        private AwardParams(CountryId votingCountryId, IReadOnlyList<CountryId> rankedCompetingCountryIds)
        {
            VotingCountryId = votingCountryId;
            RankedCompetingCountryIds = rankedCompetingCountryIds;
        }

        public CountryId VotingCountryId { get; }

        public IReadOnlyList<CountryId> RankedCompetingCountryIds { get; }

        public static AwardParams From(
            CountryId[] rankedCompetingCountryIds = null!,
            CountryId votingCountryId = null!
        ) => new(votingCountryId, rankedCompetingCountryIds);
    }
}
