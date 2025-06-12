using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
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
    public bool PointsAwarded { get; private set; }

    internal void AwardPoints(IReadOnlyList<Competitor> rankedCompetitors)
    {
        IEnumerable<PointsValue> pointsValues = Enum.GetValues<PointsValue>()
            .OrderByDescending(value => value)
            .Concat(Enumerable.Repeat(PointsValue.Zero, rankedCompetitors.Count));

        foreach (var (competitor, pointsValue) in rankedCompetitors.Zip(pointsValues))
        {
            AwardPoints(competitor, pointsValue);
        }

        PointsAwarded = true;
    }

    private protected abstract void AwardPoints(Competitor competitor, PointsValue pointsValue);
}
