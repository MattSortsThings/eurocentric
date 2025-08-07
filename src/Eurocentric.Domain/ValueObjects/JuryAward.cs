using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents an award from a jury to a competitor in a broadcast.
/// </summary>
public sealed class JuryAward : Award
{
    /// <summary>
    ///     Creates a new <see cref="JuryAward" /> instance with the provided <see cref="Award.VotingCountryId" /> and
    ///     <see cref="Award.PointsValue" /> values.
    /// </summary>
    /// <param name="votingCountryId">The ID of the country aggregate the jury represents.</param>
    /// <param name="pointsValue">The numeric points value of the award.</param>
    /// <exception cref="ArgumentNullException"><paramref name="votingCountryId" /> is <see langword="null" />.</exception>
    public JuryAward(CountryId votingCountryId, PointsValue pointsValue) : base(votingCountryId, pointsValue)
    {
    }
}
