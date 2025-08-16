using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using JetBrains.Annotations;

namespace Eurocentric.Domain.Aggregates.Broadcasts;

/// <summary>
///     Represents a voter in a broadcast.
/// </summary>
public abstract class Voter : Entity
{
    [UsedImplicitly(Reason = "EF Core")]
    private protected Voter()
    {
    }

    private protected Voter(CountryId votingCountryId)
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

    internal void AwardPoints(ICollection<Competitor> competitors)
    {
        IEnumerable<PointsValue> pointsValues = Enum.GetValues<PointsValue>()
            .OrderByDescending(value => value)
            .Concat(Enumerable.Repeat(PointsValue.Zero, competitors.Count));

        foreach ((Competitor competitor, PointsValue pointsValue) in competitors.Zip(pointsValues))
        {
            GivePointsAward(competitor, pointsValue);
        }

        PointsAwarded = true;
    }

    private protected abstract void GivePointsAward(Competitor competitor, PointsValue pointsValue);
}
