namespace Eurocentric.Apis.Public.V1.Dtos.Listings;

/// <summary>
///     A single competing country jury points listings row.
/// </summary>
public sealed record CompetingCountryJuryPointsListing
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
}
