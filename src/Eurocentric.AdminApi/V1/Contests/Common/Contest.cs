namespace Eurocentric.AdminApi.V1.Contests.Common;

public sealed record Contest(Guid Id, int ContestYear, string HostCityName, VotingRules VotingRules);
