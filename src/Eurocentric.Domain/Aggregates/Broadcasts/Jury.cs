using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a single jury in a broadcast aggregate.
/// </summary>
public sealed class Jury : Voter
{
    private Jury()
    {
    }

    public Jury(CountryId votingCountryId) : base(votingCountryId) { }
}
