namespace Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;

/// <summary>
///     A single competing country points consensus rankings row.
/// </summary>
/// <remarks>
///     Integer points award values in the range [0, 12] are normalized to floating point values in the range
///     [1.0, 10.0] to use in vector calculations.
/// </remarks>
public sealed record CompetingCountryPointsConsensusRanking
{
    /// <summary>
    ///     The competing country's rank based on descending points consensus.
    /// </summary>
    public int Rank { get; init; }

    /// <summary>
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The cosine similarity between the normalized jury and televote points the competing country received across
    ///     broadcasts, using each voting country in each broadcast as a vector dimension in the comparison.
    /// </summary>
    public decimal PointsConsensus { get; init; }

    /// <summary>
    ///     The number of vector dimensions in the queried filtered voting data for the competing country.
    /// </summary>
    public int VectorDimensions { get; init; }

    /// <summary>
    ///     The length of the normalized jury points vector.
    /// </summary>
    public decimal JuryVectorLength { get; init; }

    /// <summary>
    ///     The length of the normalized televote points vector.
    /// </summary>
    public decimal TelevoteVectorLength { get; init; }

    /// <summary>
    ///     The dot product of the normalized jury points vector and the normalized televote points vector.
    /// </summary>
    public decimal VectorDotProduct { get; init; }

    /// <summary>
    ///     The number of unique broadcasts in the queried filtered voting data for the competing country.
    /// </summary>
    public int Broadcasts { get; init; }

    /// <summary>
    ///     The number of unique contests in the queried filtered voting data for the competing country.
    /// </summary>
    public int Contests { get; init; }

    /// <summary>
    ///     The number of unique voting countries in the queried filtered voting data for the competing country.
    /// </summary>
    public int VotingCountries { get; init; }
}
