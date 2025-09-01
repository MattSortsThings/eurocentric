namespace Eurocentric.Domain.V0.Entities;

public abstract record Voter
{
    public Guid VotingCountryId { get; init; }

    public bool PointsAwarded { get; init; }
}
