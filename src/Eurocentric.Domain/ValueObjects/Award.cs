using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents an award from a voter to a competitor in a broadcast.
/// </summary>
public abstract class Award : ValueObject
{
    protected Award(CountryId votingCountryId, PointsValue pointsValue)
    {
        VotingCountryId = votingCountryId ?? throw new ArgumentNullException(nameof(votingCountryId));
        PointsValue = pointsValue;
    }

    /// <summary>
    ///     Gets the ID of the voting country for the award.
    /// </summary>
    public CountryId VotingCountryId { get; }

    /// <summary>
    ///     Gets the award's points value.
    /// </summary>
    public PointsValue PointsValue { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return VotingCountryId;
        yield return PointsValue;
    }
}
