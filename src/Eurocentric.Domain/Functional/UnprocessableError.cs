namespace Eurocentric.Domain.Functional;

/// <summary>
///     An error that occurs when the request cannot be processed, irrespective of the current state of the requested
///     aggregate (if applicable) and all other aggregates in the system.
/// </summary>
public sealed record UnprocessableError : IDomainError
{
    /// <inheritdoc />
    public required string Title { get; init; }

    /// <inheritdoc />
    public required string Detail { get; init; }

    /// <inheritdoc />
    public required IReadOnlyDictionary<string, object?>? Extensions { get; init; }
}
