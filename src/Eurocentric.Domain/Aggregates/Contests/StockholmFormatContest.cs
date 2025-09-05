using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a "Stockholm" format contest.
/// </summary>
public sealed class StockholmFormatContest : Contest
{
    [UsedImplicitly(Reason = "EF Core")]
    private StockholmFormatContest()
    {
        ContestFormat = ContestFormat.Stockholm;
    }

    public StockholmFormatContest(ContestId id,
        ContestYear contestYear,
        CityName cityName,
        List<Participant> participants) : base(id, contestYear, cityName, participants)
    {
        ContestFormat = ContestFormat.Stockholm;
    }

    /// <inheritdoc />
    public override ContestFormat ContestFormat { get; private protected init; }
}
