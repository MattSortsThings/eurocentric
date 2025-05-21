using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a voting country awarding jury points in a broadcast.
/// </summary>
public sealed class Jury : Voter
{
    private Jury()
    {
    }

    public Jury(CountryId votingCountryId) : base(votingCountryId) { }
}
