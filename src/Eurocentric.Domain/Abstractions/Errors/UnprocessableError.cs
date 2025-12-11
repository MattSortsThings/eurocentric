using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Abstractions.Errors;

/// <summary>
///     A domain error arose because the request was impossible, irrespective of the current state of all existing
///     aggregates.
/// </summary>
public abstract record UnprocessableError : IDomainError
{
    /// <inheritdoc />
    public DomainErrorType Type => DomainErrorType.Unprocessable;

    /// <inheritdoc />
    public abstract string Title { get; }

    /// <inheritdoc />
    public abstract string Detail { get; }

    /// <inheritdoc />
    public abstract IReadOnlyDictionary<string, object?>? Extensions { get; }
}
