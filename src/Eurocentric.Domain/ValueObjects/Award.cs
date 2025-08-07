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
    ///     Gets the ID of the country aggregate the voter represents.
    /// </summary>
    public CountryId VotingCountryId { get; }

    /// <summary>
    ///     Gets the numeric points value of the award.
    /// </summary>
    public PointsValue PointsValue { get; }

    private protected override IEnumerable<object> GetAtomicValues()
    {
        yield return VotingCountryId;
        yield return PointsValue;
    }
}
