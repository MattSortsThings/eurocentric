using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents a single points award from a jury to a competitor in a broadcast.
/// </summary>
public sealed class JuryAward : Award
{
    public JuryAward(CountryId votingCountryId, PointsValue pointsValue) : base(votingCountryId, pointsValue)
    {
    }
}
