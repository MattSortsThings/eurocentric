using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Contests;

/// <summary>
///     Represents a contest aggregate using the Stockholm contest format.
/// </summary>
public sealed class StockholmFormatContest : Contest
{
    private StockholmFormatContest()
    {
    }

    public StockholmFormatContest(ContestId id, ContestYear year, CityName cityName, List<Participant> participants) :
        base(id, year, cityName, participants)
    {
    }

    /// <inheritdoc />
    public override ContestFormat Format { get; private protected init; } = ContestFormat.Stockholm;
}
