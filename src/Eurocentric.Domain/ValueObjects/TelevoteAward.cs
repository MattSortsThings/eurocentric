using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;

namespace Eurocentric.Domain.ValueObjects;

/// <summary>
///     Represents a single points award from a televote to a competitor in a broadcast.
/// </summary>
public sealed class TelevoteAward : Award
{
    public TelevoteAward(CountryId votingCountryId, PointsValue pointsValue) : base(votingCountryId, pointsValue)
    {
    }
}
