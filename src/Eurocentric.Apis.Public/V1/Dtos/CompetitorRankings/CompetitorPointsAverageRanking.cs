using Eurocentric.Apis.Public.V1.Enums;
using Eurocentric.Components.OpenApi;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

/// <summary>
///     A single competitor points average rankings row.
/// </summary>
public sealed record CompetitorPointsAverageRanking : IDtoSchemaExampleProvider<CompetitorPointsAverageRanking>
{
    /// <summary>
    ///     The competitor's rank based on descending points average.
    /// </summary>
    public int Rank { get; init; }

    /// <summary>
    ///     The contest year of the broadcast.
    /// </summary>
    public int ContestYear { get; init; }

    /// <summary>
    ///     The broadcast's stage in the contest.
    /// </summary>
    public ContestStage ContestStage { get; init; }

    /// <summary>
    ///     The competitor's running order spot in the broadcast.
    /// </summary>
    public int RunningOrderSpot { get; init; }

    /// <summary>
    ///     The competing country's ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    ///     The competing country's short UK English name.
    /// </summary>
    public string CountryName { get; init; } = string.Empty;

    /// <summary>
    ///     The competitor's finishing position in the broadcast.
    /// </summary>
    public int FinishingPosition { get; init; }

    /// <summary>
    ///     The competitor's act name.
    /// </summary>
    public string ActName { get; init; } = string.Empty;

    /// <summary>
    ///     The competitor's song title.
    /// </summary>
    public string SongTitle { get; init; } = string.Empty;

    /// <summary>
    ///     The average individual points value the competitor received in the broadcast.
    /// </summary>
    public decimal PointsAverage { get; init; }

    /// <summary>
    ///     The sum total points the competitor received in the broadcast.
    /// </summary>
    public int TotalPoints { get; init; }

    /// <summary>
    ///     The number of points awards in the queried filtered voting data for the competitor.
    /// </summary>
    public int PointsAwards { get; init; }

    /// <summary>
    ///     The number of unique voting countries in the queried filtered voting data for the competitor.
    /// </summary>
    public int VotingCountries { get; init; }

    public static CompetitorPointsAverageRanking CreateExample() =>
        new()
        {
            Rank = 1,
            ContestYear = 2025,
            ContestStage = ContestStage.GrandFinal,
            CountryCode = "AA",
            CountryName = "CountryName",
            ActName = "ActName",
            SongTitle = "SongTitle",
            PointsAverage = 5.0m,
            TotalPoints = 375,
            PointsAwards = 75,
            VotingCountries = 38,
        };

    public bool Equals(CompetitorPointsAverageRanking? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Rank == other.Rank
            && ContestYear == other.ContestYear
            && ContestStage == other.ContestStage
            && RunningOrderSpot == other.RunningOrderSpot
            && CountryCode == other.CountryCode
            && CountryName == other.CountryName
            && FinishingPosition == other.FinishingPosition
            && ActName == other.ActName
            && SongTitle == other.SongTitle
            && PointsAverage == other.PointsAverage
            && TotalPoints == other.TotalPoints
            && PointsAwards == other.PointsAwards
            && VotingCountries == other.VotingCountries;
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Rank);
        hashCode.Add(ContestYear);
        hashCode.Add((int)ContestStage);
        hashCode.Add(RunningOrderSpot);
        hashCode.Add(CountryCode);
        hashCode.Add(CountryName);
        hashCode.Add(FinishingPosition);
        hashCode.Add(ActName);
        hashCode.Add(SongTitle);
        hashCode.Add(PointsAverage);
        hashCode.Add(TotalPoints);
        hashCode.Add(PointsAwards);
        hashCode.Add(VotingCountries);

        return hashCode.ToHashCode();
    }
}
