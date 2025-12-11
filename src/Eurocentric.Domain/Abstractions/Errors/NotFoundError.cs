using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Abstractions.Errors;

/// <summary>
///     A domain error that arose because the requested aggregate was not found.
/// </summary>
public abstract record NotFoundError : IDomainError
{
    /// <inheritdoc />
    public DomainErrorType Type => DomainErrorType.NotFound;

    /// <inheritdoc />
    public abstract string Title { get; }

    /// <inheritdoc />
    public abstract string Detail { get; }

    /// <inheritdoc />
    public abstract IReadOnlyDictionary<string, object?>? Extensions { get; }
}
