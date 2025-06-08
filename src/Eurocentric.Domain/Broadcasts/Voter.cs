using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a voter that awards a set of points in a broadcast.
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
    public bool PointsAwarded { get; private set; } = false;
}
