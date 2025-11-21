using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     A single competing country televote points listings row.
/// </summary>
public sealed record CompetingCountryTelevotePointsListing
    : IDtoSchemaExampleProvider<CompetingCountryTelevotePointsListing>
{
    /// <summary>
    ///     The value of the jury points award the competing country received from the voting country.
    /// </summary>
    public int PointsValue { get; init; }

    /// <summary>
    ///     The voting country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string VotingCountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The voting country's short UK English name.
    /// </summary>
    public string VotingCountryName { get; init; } = string.Empty;

    public static CompetingCountryTelevotePointsListing CreateExample() =>
        new()
        {
            PointsValue = 10,
            VotingCountryCode = "AA",
            VotingCountryName = "CountryName",
        };

    public bool Equals(CompetingCountryTelevotePointsListing? other)
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
            && VotingCountryCode == other.VotingCountryCode
            && VotingCountryName == other.VotingCountryName;
    }

    public override int GetHashCode() => HashCode.Combine(PointsValue, VotingCountryCode, VotingCountryName);
}
