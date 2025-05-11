using Eurocentric.Domain.Enums;

namespace Eurocentric.Infrastructure.FakeRepositories;

public sealed record FakeVoterPointsDatum(
    int ContestYear,
    ContestStage ContestStage,
    string VotingCountryCode,
    string VotingCountryName,
    string CompetingCountryCode,
    int PointsAwarded,
    int AvailablePoints);
