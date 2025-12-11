using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Abstractions.Errors;

/// <summary>
///     An error that occurs in the domain.
/// </summary>
public interface IDomainError
{
    /// <summary>
    ///     Gets the domain error's type.
    /// </summary>
    DomainErrorType Type { get; }

    /// <summary>
    ///     Gets the domain error's simple title.
    /// </summary>
    string Title { get; }

    /// <summary>
    ///     Gets the domain error's detailed description.
    /// </summary>
    string Detail { get; }

    /// <summary>
    ///     Gets the domain error's optional extensions.
    /// </summary>
    IReadOnlyDictionary<string, object?>? Extensions { get; }
}
