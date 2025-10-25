using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest using the "Liverpool" contest rules.
/// </summary>
public sealed class LiverpoolRulesContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private LiverpoolRulesContest() { }

    private LiverpoolRulesContest(
        ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants,
        GlobalTelevote? globalTelevote
    )
        : base(id, contestYear, cityName, participants, globalTelevote) { }

    public override ContestRules ContestRules { get; private protected init; } = ContestRules.Liverpool;

    public static LiverpoolRulesContest CreateDummyContest(Guid idValue, int contestYearValue, string cityNameValue)
    {
        ContestId id = ContestId.FromValue(idValue);
        ContestYear? contestYear = ContestYear.FromValue(contestYearValue).GetValueOrDefault();
        CityName? cityName = CityName.FromValue(cityNameValue).GetValueOrDefault();
        GlobalTelevote globalTelevote = new(CountryId.FromValue(Guid.NewGuid()));

        List<Participant> participants = Enumerable
            .Range(0, 10)
            .Select(i =>
            {
                CountryId countryId = CountryId.FromValue(Guid.NewGuid());
                ActName actName = ActName.FromValue("ActName ").GetValueOrDefault();
                SongTitle songTitle = SongTitle.FromValue("SongTitle").GetValueOrDefault();
                SemiFinalDraw draw = i % 2 == 0 ? SemiFinalDraw.SemiFinal1 : SemiFinalDraw.SemiFinal2;

                return new Participant(countryId, draw, actName, songTitle);
            })
            .ToList();

        return new LiverpoolRulesContest(id, contestYear, cityName, participants, globalTelevote);
    }
}
