namespace Eurocentric.PublicApi.V1.VotingCountryRankings.Models;

public sealed record VotingCountryPointsShareItem(
    int Rank,
    string CountryCode,
    double PointsShare,
    int TotalPoints,
    int PossiblePoints,
    string CountryName);
