namespace Eurocentric.Domain.Errors;

/// <summary>
///     An error that occurs while handling a request.
/// </summary>
public sealed record DomainError
{
    /// <summary>
    ///     Gets or initializes the domain error's type.
    /// </summary>
    public required DomainErrorType Type { get; init; }

    /// <summary>
    ///     Gets or initializes the domain error's unique title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    ///     Gets or initializes the domain error's description.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    ///     Gets or initializes the domain error's optional additional data.
    /// </summary>
    public IReadOnlyDictionary<string, object?>? AdditionalData { get; init; }
}
