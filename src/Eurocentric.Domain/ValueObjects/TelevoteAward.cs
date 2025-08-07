using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents an award from a televote to a competitor in a broadcast.
/// </summary>
public sealed class TelevoteAward : Award
{
    /// <summary>
    ///     Creates a new <see cref="TelevoteAward" /> instance with the provided <see cref="Award.VotingCountryId" /> and
    ///     <see cref="Award.PointsValue" /> values.
    /// </summary>
    /// <param name="votingCountryId">The ID of the country aggregate the televote represents.</param>
    /// <param name="pointsValue">The numeric points value of the award.</param>
    /// <exception cref="ArgumentNullException"><paramref name="votingCountryId" /> is <see langword="null" />.</exception>
    public TelevoteAward(CountryId votingCountryId, PointsValue pointsValue) : base(votingCountryId, pointsValue)
    {
    }
}
