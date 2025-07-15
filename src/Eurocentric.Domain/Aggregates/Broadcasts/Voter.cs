using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a single voter in a broadcast aggregate.
/// </summary>
public abstract class Voter : Entity
{
    private protected Voter()
    {
    }

    protected Voter(CountryId votingCountryId)
    {
        VotingCountryId = votingCountryId;
    }

    /// <summary>
    ///     Gets the ID of the country aggregate that the voter represents.
    /// </summary>
    public CountryId VotingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets a boolean value indicating whether the voter has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; private set; }
}
