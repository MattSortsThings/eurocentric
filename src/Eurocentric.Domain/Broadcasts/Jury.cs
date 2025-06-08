using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a jury that awards a set of points in a broadcast.
/// </summary>
public sealed class Jury : Voter
{
    private Jury()
    {
    }

    public Jury(CountryId votingCountryId) : base(votingCountryId)
    {
    }
}
