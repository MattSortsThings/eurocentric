namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

/// <summary>
///     A single voting country points consensus rankings row.
/// </summary>
/// <remarks>
///     Integer points award values in the range [0, 12] are normalized to floating point values in the range
///     [1.0, 10.0] to use in vector calculations.
/// </remarks>
public sealed record VotingCountryPointsConsensusRanking
{
    /// <summary>
    ///     The voting country's rank based on descending points consensus.
    /// </summary>
    public int Rank { get; init; }

    /// <summary>
    ///     The voting country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The voting country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The cosine similarity between the normalized jury and televote points the voting country gave across
    ///     broadcasts, using each competing country in each broadcast as a vector dimension in the comparison.
    /// </summary>
    public decimal PointsConsensus { get; init; }

    /// <summary>
    ///     The number of vector dimensions in the comparison.
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
    ///     The number of unique broadcasts in the queried voting data for the voting country.
    /// </summary>
    public int Broadcasts { get; init; }

    /// <summary>
    ///     The number of unique contests in the queried voting data for the voting country.
    /// </summary>
    public int Contests { get; init; }

    /// <summary>
    ///     The number of unique competing countries in the queried voting data for the voting country.
    /// </summary>
    public int CompetingCountries { get; init; }
}
