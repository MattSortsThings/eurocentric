namespace Eurocentric.Domain.Core;

/// <summary>
///     An error that occurs when the request cannot be processed given the current state of the requested aggregate (if
///     applicable) and all other aggregates in the system.
/// </summary>
public sealed record ConflictError : IDomainError
{
    /// <inheritdoc />
    public required string Title { get; init; }

    /// <inheritdoc />
    public required string Detail { get; init; }

    /// <inheritdoc />
    public required IReadOnlyDictionary<string, object?>? Extensions { get; init; }
}
