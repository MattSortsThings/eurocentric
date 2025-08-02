namespace Eurocentric.Domain.V0Entities;

public sealed record Country
{
    public Guid Id { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public IList<ContestMemo> ParticipatingContestIds { get; init; } = [];
}
