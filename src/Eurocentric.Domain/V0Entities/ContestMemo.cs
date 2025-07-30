namespace Eurocentric.Domain.V0Entities;

public sealed record ContestMemo
{
    public required Guid ContestId { get; init; }
}
