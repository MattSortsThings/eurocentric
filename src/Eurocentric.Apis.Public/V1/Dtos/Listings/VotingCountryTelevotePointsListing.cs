using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     A single voting country televote points listings row.
/// </summary>
public sealed record VotingCountryTelevotePointsListing : IDtoSchemaExampleProvider<VotingCountryTelevotePointsListing>
{
    /// <summary>
    ///     The value of the televote points award the voting country gave to the competing country.
    /// </summary>
    public int PointsValue { get; init; }

    /// <summary>
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CompetingCountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CompetingCountryName { get; init; } = string.Empty;

    public static VotingCountryTelevotePointsListing CreateExample() =>
        new()
        {
            PointsValue = 10,
            CompetingCountryCode = "AA",
            CompetingCountryName = "CountryName",
        };

    public bool Equals(VotingCountryTelevotePointsListing? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return PointsValue == other.PointsValue
            && CompetingCountryCode == other.CompetingCountryCode
            && CompetingCountryName == other.CompetingCountryName;
    }

    public override int GetHashCode() => HashCode.Combine(PointsValue, CompetingCountryCode, CompetingCountryName);
}
