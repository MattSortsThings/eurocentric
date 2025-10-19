namespace Eurocentric.Domain.Functional;

public sealed record NotFoundError : IDomainError
{
    public required string Title { get; init; }

    public required string Detail { get; init; }

    public required IReadOnlyDictionary<string, object?>? Metadata { get; init; }
}
