using Eurocentric.Domain.Identifiers;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a jury in a broadcast.
/// </summary>
public sealed class Jury : Voter
{
    [UsedImplicitly(Reason = "EF Core")]
    private Jury()
    {
    }

    public Jury(CountryId votingCountryId) : base(votingCountryId)
    {
    }
}
