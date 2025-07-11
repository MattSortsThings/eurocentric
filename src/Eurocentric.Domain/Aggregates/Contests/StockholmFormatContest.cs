using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate using the "Stockholm" contest format.
/// </summary>
public sealed class StockholmFormatContest : Contest
{
    private StockholmFormatContest()
    {
    }

    public StockholmFormatContest(ContestId id, List<Participant> participants, ContestYear contestYear, CityName cityName) :
        base(id, participants, contestYear, cityName)
    {
    }

    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Stockholm;
}
