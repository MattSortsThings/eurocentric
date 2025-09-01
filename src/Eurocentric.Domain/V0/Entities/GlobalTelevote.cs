namespace Eurocentric.Domain.V0.Entities;

public sealed record GlobalTelevote
{
    public Guid ParticipatingCountryId { get; init; }
}
