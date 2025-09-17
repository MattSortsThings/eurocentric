namespace Eurocentric.Domain.V0Entities;

public sealed record GlobalTelevote
{
    public Guid VotingCountryId { get; init; }
}
