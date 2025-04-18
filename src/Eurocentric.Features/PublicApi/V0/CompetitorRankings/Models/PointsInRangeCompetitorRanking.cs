namespace Eurocentric.Features.PublicApi.V0.CompetitorRankings.Models;

public sealed record PointsInRangeCompetitorRanking(
    int Rank,
    string CountryCode,
    string CountryName,
    double PointsInRangeFrequency);
