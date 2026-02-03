namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class Jury
{
    public required Guid VotingCountryId { get; init; }

    public required bool PointsAwarded { get; init; }
}
