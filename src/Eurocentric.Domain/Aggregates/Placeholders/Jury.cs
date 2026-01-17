namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Jury
{
    public Guid VotingCountryId { get; init; }

    public bool PointsAwarded { get; init; }
}
