namespace Eurocentric.Domain.V0Entities;

public sealed record Country
{
    public required Guid Id { get; init; }

    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required IList<ContestMemo> ParticipatingContestIds { get; init; }
}
