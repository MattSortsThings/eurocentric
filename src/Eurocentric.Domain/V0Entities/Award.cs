using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.V0Entities;

public abstract record Award
{
    public Guid VotingCountryId { get; init; }

    public PointsValue PointsValue { get; init; }
}
