using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents a single award from a jury to a competitor in a broadcast.
/// </summary>
public sealed class JuryAward : PointsAward
{
    /// <summary>
    ///     Creates and returns a new <see cref="JuryAward" />.
    /// </summary>
    /// <param name="votingCountryId">The ID of the voting country for the award.</param>
    /// <param name="pointsValue">The points value of the award.</param>
    /// <exception cref="ArgumentNullException"><paramref name="votingCountryId" /> is <see langword="null" />.</exception>
    public JuryAward(CountryId votingCountryId, PointsValue pointsValue) : base(votingCountryId, pointsValue)
    {
    }
}
