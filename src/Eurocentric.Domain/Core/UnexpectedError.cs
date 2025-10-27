namespace Eurocentric.Domain.Core;

/// <summary>
///     An error that occurs when an unexpected error occurs while handling a request.
/// </summary>
public sealed record UnexpectedError : IDomainError
{
    /// <inheritdoc />
    public required string Title { get; init; }

    /// <inheritdoc />
    public required string Detail { get; init; }

    /// <inheritdoc />
    public required IReadOnlyDictionary<string, object?>? Extensions { get; init; }
}
