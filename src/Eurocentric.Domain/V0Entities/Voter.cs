namespace Eurocentric.Domain.V0Entities;

public abstract record Voter
{
    public Guid VotingCountryId { get; init; }

    public bool PointsAwarded { get; set; }
}
