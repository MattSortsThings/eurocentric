using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest using the "Stockholm" contest rules.
/// </summary>
public sealed class StockholmRulesContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private StockholmRulesContest() { }

    private StockholmRulesContest(
        ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants
    )
        : base(id, contestYear, cityName, participants) { }

    public override ContestRules ContestRules { get; private protected init; } = ContestRules.Stockholm;

    public static StockholmRulesContest CreateDummyContest(Guid idValue, int contestYearValue, string cityNameValue)
    {
        ContestId id = ContestId.FromValue(idValue);
        ContestYear? contestYear = ContestYear.FromValue(contestYearValue).GetValueOrDefault();
        CityName? cityName = CityName.FromValue(cityNameValue).GetValueOrDefault();

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

        return new StockholmRulesContest(id, contestYear, cityName, participants);
    }
}
