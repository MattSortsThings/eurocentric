using Eurocentric.Domain.Core;
using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     A points award given by a televote in a broadcast.
/// </summary>
public sealed class TelevoteAward : CompoundValueObject
{
    /// <summary>
    ///     Initializes a new <see cref="TelevoteAward" /> instance.
    /// </summary>
    /// <param name="votingCountryId">The ID of the voting country that gives the award.</param>
    /// <param name="pointsValue">The numeric points value of the award.</param>
    /// <exception cref="ArgumentNullException"><paramref name="votingCountryId" /> is <see langword="null" />.</exception>
    public TelevoteAward(CountryId votingCountryId, PointsValue pointsValue)
    {
        VotingCountryId = votingCountryId ?? throw new ArgumentNullException(nameof(votingCountryId));
        PointsValue = pointsValue;
    }

    /// <summary>
    ///     Gets the ID of the voting country that gives the award.
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
