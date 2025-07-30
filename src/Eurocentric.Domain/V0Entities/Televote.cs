namespace Eurocentric.Domain.V0Entities;

public sealed record Televote
{
    public required Guid VotingCountryId { get; init; }

    public required bool PointsAwarded { get; init; }
}
