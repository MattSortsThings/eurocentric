using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Aggregates.Contests;

/// <summary>
///     Represents a contest aggregate using the "Liverpool" contest format.
/// </summary>
public sealed class LiverpoolFormatContest : Contest
{
    private LiverpoolFormatContest()
    {
    }

    public LiverpoolFormatContest(ContestId id, List<Participant> participants, ContestYear contestYear, CityName cityName) :
        base(id, participants, contestYear, cityName)
    {
    }

    public override ContestFormat ContestFormat { get; private protected init; } = ContestFormat.Liverpool;
}
