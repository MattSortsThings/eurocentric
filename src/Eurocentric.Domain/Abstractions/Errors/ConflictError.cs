using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Abstractions.Errors;

/// <summary>
///     A domain error arose because the request was impossible given the current state of all existing aggregates.
/// </summary>
public abstract record ConflictError : IDomainError
{
    /// <inheritdoc />
    public DomainErrorType Type => DomainErrorType.Conflict;

    /// <inheritdoc />
    public abstract string Title { get; }

    /// <inheritdoc />
    public abstract string Detail { get; }

    /// <inheritdoc />
    public abstract IReadOnlyDictionary<string, object?>? Extensions { get; }
}
