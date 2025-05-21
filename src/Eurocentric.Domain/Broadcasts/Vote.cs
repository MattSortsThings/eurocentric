using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.Broadcasts;

/// <summary>
///     Represents a voting country awarding points in a broadcast.
/// </summary>
public abstract class Vote : Entity
{
    private protected Vote()
    {
    }

    protected internal Vote(CountryId votingCountryId)
    {
        VotingCountryId = votingCountryId;
    }

    /// <summary>
    ///     Gets the ID of the voting country.
    /// </summary>
    public CountryId VotingCountryId { get; private init; } = null!;

    /// <summary>
    ///     Gets a boolean value indicating whether the vote has awarded its points.
    /// </summary>
    public bool PointsAwarded { get; private set; }
}
