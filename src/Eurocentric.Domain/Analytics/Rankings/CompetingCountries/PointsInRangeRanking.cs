namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

/// <summary>
///     A single competing country points in range rankings row.
/// </summary>
public sealed record PointsInRangeRanking
{
    /// <summary>
    ///     The competing country's rank based on descending points in range.
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
    ///     The frequency of points awards within the specified value range the competing country received across broadcasts,
    ///     relative to the number of points awards the competing country received.
    /// </summary>
    public decimal PointsInRange { get; init; }

    /// <summary>
    ///     The number of points awards within the specified value range the competing country received across broadcasts.
    /// </summary>
    public int PointsAwardsInRange { get; init; }

    /// <summary>
    ///     The number of points awards in the queried filtered voting data for the competing country.
    /// </summary>
    public int PointsAwards { get; init; }

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
