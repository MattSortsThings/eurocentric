namespace Eurocentric.Domain.V0Entities;

public sealed record Jury
{
    public Guid VotingCountryId { get; init; }

    public bool PointsAwarded { get; init; }
}
