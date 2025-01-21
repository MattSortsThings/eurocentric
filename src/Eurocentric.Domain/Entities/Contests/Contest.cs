namespace Eurocentric.Domain.Entities.Contests;

public sealed record Contest(Guid Id, int ContestYear, string HostCityName, VotingRules VotingRules);
