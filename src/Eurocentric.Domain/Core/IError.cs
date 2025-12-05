namespace Eurocentric.Domain.Core;

/// <summary>
///     An error that occurs in the domain while handling a request.
/// </summary>
public interface IError
{
    /// <summary>
    ///     Gets the error's title.
    /// </summary>
    string Title { get; }

    /// <summary>
    ///     Gets the error's detail.
    /// </summary>
    string Detail { get; }

    /// <summary>
    ///     Gets the error's optional extensions.
    /// </summary>
    Dictionary<string, object?>? Extensions { get; }
}
