using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a voter in a broadcast.
/// </summary>
public abstract class Voter : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private protected Voter() { }

    internal Voter(CountryId votingCountryId)
    {
        VotingCountryId = votingCountryId;
    }

    /// <summary>
    ///     Gets the ID of the voting country.
    /// </summary>
    public CountryId VotingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets a boolean value indicating whether the voter has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; private set; }
}
