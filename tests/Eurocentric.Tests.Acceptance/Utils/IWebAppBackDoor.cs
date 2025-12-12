namespace Eurocentric.Tests.Acceptance.Utils;

/// <summary>
///     Allows the user to interact with the in-memory web application bypassing its web API surface.
/// </summary>
public interface IWebAppBackDoor
{
    /// <summary>
    ///     Executes the provided function within a new asynchronous service scope.
    /// </summary>
    /// <param name="func">
    ///     An asynchronous function, executed on the web application's scoped service provider, which does not
    ///     return a value.
    /// </param>
    /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
    Task ExecuteScopedAsync(Func<IServiceProvider, Task> func);

    /// <summary>
    ///     Executes the provided function within a new asynchronous service scope and returns the result.
    /// </summary>
    /// <param name="func">
    ///     An asynchronous function, executed on the web application's scoped service provider, which returns a
    ///     value.
    /// </param>
    /// <typeparam name="T">The return value type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> that represents the asynchronous operation. The task's result is the function's
    ///     return value.
    /// </returns>
    Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func);
}
