using Eurocentric.Domain.Identifiers;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a televote in a broadcast.
/// </summary>
public sealed class Televote : Voter
{
    [UsedImplicitly(Reason = "EF Core")]
    private Televote()
    {
    }

    public Televote(CountryId votingCountryId) : base(votingCountryId)
    {
    }
}
