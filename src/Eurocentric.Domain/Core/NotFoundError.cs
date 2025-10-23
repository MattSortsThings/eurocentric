namespace Eurocentric.Domain.Core;

/// <summary>
///     An error that occurs when the requested aggregate is not found.
/// </summary>
public sealed record NotFoundError : IDomainError
{
    /// <inheritdoc />
    public required string Title { get; init; }

    /// <inheritdoc />
    public required string Detail { get; init; }

    /// <inheritdoc />
    public required IReadOnlyDictionary<string, object?>? Extensions { get; init; }
}
