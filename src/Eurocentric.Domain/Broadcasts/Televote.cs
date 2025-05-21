using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a voting country awarding televote points in a broadcast.
/// </summary>
public sealed class Televote : Voter
{
    private Televote()
    {
    }

    public Televote(CountryId votingCountryId) : base(votingCountryId) { }
}
