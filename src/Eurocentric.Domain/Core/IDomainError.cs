namespace Eurocentric.Domain.Core;

/// <summary>
///     An error that occurs in the domain.
/// </summary>
public interface IDomainError
{
    /// <summary>
    ///     Gets the error's unique title.
    /// </summary>
    string Title { get; }

    /// <summary>
    ///     Gets the error's detail.
    /// </summary>
    string Detail { get; }

    /// <summary>
    ///     Gets the error's optional extensions.
    /// </summary>
    IReadOnlyDictionary<string, object?>? Extensions { get; }
}
