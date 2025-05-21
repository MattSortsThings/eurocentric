using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents a single points award from a vote to a competitor in a broadcast.
/// </summary>
public abstract class Award : ValueObject
{
    protected Award(CountryId votingCountryId, PointsValue pointsValue)
    {
        VotingCountryId = votingCountryId;
        PointsValue = pointsValue;
    }

    /// <summary>
    ///     Gets the ID of the voting country that gave the award.
    /// </summary>
    public CountryId VotingCountryId { get; init; }

    /// <summary>
    ///     Gets the points value of the award.
    /// </summary>
    public PointsValue PointsValue { get; init; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return VotingCountryId;
        yield return PointsValue;
    }
}
