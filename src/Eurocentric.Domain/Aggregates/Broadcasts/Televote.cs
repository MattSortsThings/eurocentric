using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a single televote in a broadcast aggregate.
/// </summary>
public sealed class Televote : Voter
{
    private Televote()
    {
    }

    public Televote(CountryId votingCountryId) : base(votingCountryId) { }
}
