namespace Eurocentric.AdminApi.V1.Contests.Models;

public sealed record Contest(Guid Id, int ContestYear, string HostCityName, VotingRules VotingRules);
