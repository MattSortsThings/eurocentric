namespace Eurocentric.PublicApi.V0.VotingCountryRankings.Models;

public sealed record VotingCountryPointsShareRanking(
    int Rank,
    string CountryCode,
    double PointsShare,
    int TotalPoints,
    int PossiblePoints,
    string CountryName);
