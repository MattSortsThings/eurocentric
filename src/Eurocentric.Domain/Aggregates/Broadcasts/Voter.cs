using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a voter in a broadcast.
/// </summary>
public class Voter : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private protected Voter()
    {
    }

    protected Voter(CountryId votingCountryId)
    {
        VotingCountryId = votingCountryId;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate the voter represents.
    /// </summary>
    public CountryId VotingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets a boolean value indicating whether the voter has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; private set; }
}
