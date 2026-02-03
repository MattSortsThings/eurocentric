using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Errors;

/// <summary>
///     Describes an error that has occurred in the domain.
/// </summary>
public abstract record DomainError
{
    /// <summary>
    ///     Gets the domain error's unique title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    ///     Gets the domain error's type.
    /// </summary>
    public abstract required DomainErrorType Type { get; init; }

    /// <summary>
    ///     Gets the domain error's description.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    ///     Gets the domain error's optional additional data.
    /// </summary>
    /// <remarks>
    ///     This is the only property on the <see cref="DomainError" /> class that contains values specific to the
    ///     request that produced the error.
    /// </remarks>
    public IReadOnlyDictionary<string, object?>? AdditionalData { get; init; }
}
