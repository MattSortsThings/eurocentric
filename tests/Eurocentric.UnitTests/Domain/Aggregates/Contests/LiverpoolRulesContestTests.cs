using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests;

public sealed partial class LiverpoolRulesContestTests : UnitTest
{
    private static readonly BroadcastId DefaultBroadcastId = BroadcastId.FromValue(
        Guid.Parse("94f10214-372a-4c67-9328-8309341aadb9")
    );

    private static readonly ContestId DefaultContestId = ContestId.FromValue(
        Guid.Parse("fe901f2e-35ab-4c3a-a828-8d0fd019a182")
    );

    private static readonly BroadcastDate BroadcastDate2016JanFirst = BroadcastDate
        .FromValue(new DateOnly(2016, 01, 01))
        .GetValueOrDefault();

    private static readonly ContestYear ContestYear2016 = ContestYear.FromValue(2016).GetValueOrDefault();
    private static readonly CityName DefaultCityName = CityName.FromValue("CityName").GetValueOrDefault();
    private static readonly ActName DefaultActName = ActName.FromValue("ActName").GetValueOrDefault();
    private static readonly SongTitle DefaultSongTitle = SongTitle.FromValue("SongTitle").GetValueOrDefault();

    private static LiverpoolRulesContest CreateALiverpoolRulesContest(
        CountryId globalTelevoteCountryId = null!,
        CountryId[] semiFinal2CountryIds = null!,
        CountryId[] semiFinal1CountryIds = null!,
        ContestYear? contestYear = null,
        ContestId? contestId = null
    )
    {
        Func<ContestId> idProvider = contestId is not null ? () => contestId : () => DefaultContestId;

        IContestBuilder builder = LiverpoolRulesContest
            .Create()
            .WithContestYear(contestYear ?? ContestYear2016)
            .WithCityName(DefaultCityName)
            .WithGlobalTelevote(globalTelevoteCountryId);

        foreach (CountryId countryId in semiFinal1CountryIds)
        {
            builder.AddSemiFinal1Participant(countryId, DefaultActName, DefaultSongTitle);
        }

        foreach (CountryId countryId in semiFinal2CountryIds)
        {
            builder.AddSemiFinal2Participant(countryId, DefaultActName, DefaultSongTitle);
        }

        Contest contest = builder.Build(idProvider).GetValueOrDefault();

        _ = contest.DetachAllDomainEvents();

        return contest as LiverpoolRulesContest ?? throw new InvalidOperationException("Build failed.");
    }
}
