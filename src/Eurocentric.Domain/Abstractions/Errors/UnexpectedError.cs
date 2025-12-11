using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Abstractions.Errors;

/// <summary>
///     An unexpected domain error.
/// </summary>
public abstract record UnexpectedError : IDomainError
{
    /// <inheritdoc />
    public DomainErrorType Type => DomainErrorType.Unexpected;

    /// <inheritdoc />
    public abstract string Title { get; }

    /// <inheritdoc />
    public abstract string Detail { get; }

    /// <inheritdoc />
    public abstract IReadOnlyDictionary<string, object?>? Extensions { get; }
}
