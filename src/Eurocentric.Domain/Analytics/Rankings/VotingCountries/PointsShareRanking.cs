namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

/// <summary>
///     A single voting country points share rankings row.
/// </summary>
public sealed record PointsShareRanking
{
    /// <summary>
    ///     The voting country's rank based on descending points share.
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
    ///     The sum total points the voting country gave to the specified competing country across broadcasts, as a
    ///     fraction of the available points.
    /// </summary>
    public decimal PointsShare { get; init; }

    /// <summary>
    ///     The sum total points the voting country gave to the specified competing country across broadcasts.
    /// </summary>
    public int TotalPoints { get; init; }

    /// <summary>
    ///     The maximum available points the voting country could have given to the specified competing country across
    ///     broadcasts.
    /// </summary>
    public int AvailablePoints { get; init; }

    /// <summary>
    ///     The number of points awards in the queried filtered voting data for the voting country.
    /// </summary>
    public int PointsAwards { get; init; }

    /// <summary>
    ///     The number of unique broadcasts in the queried filtered voting data for the voting country.
    /// </summary>
    public int Broadcasts { get; init; }

    /// <summary>
    ///     The number of unique contests in the queried filtered voting data for the voting country.
    /// </summary>
    public int Contests { get; init; }
}
