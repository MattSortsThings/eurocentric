using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a jury in a broadcast.
/// </summary>
public sealed class Jury : Voter
{
    [UsedImplicitly(Reason = "EF Core")]
    private Jury() { }

    internal Jury(CountryId votingCountryId)
        : base(votingCountryId) { }
}
