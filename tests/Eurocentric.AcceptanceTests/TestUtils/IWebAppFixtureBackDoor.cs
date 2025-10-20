namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Allows the user to change the state of the web application fixture using operations on its scoped service
///     provider.
/// </summary>
public interface IWebAppFixtureBackDoor
{
    /// <summary>
    ///     Executes the provided operation on the web application fixture's service provider using a new asynchronous service
    ///     scope.
    /// </summary>
    /// <param name="func">The operation to be executed within the service scope.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task ExecuteScopedAsync(Func<IServiceProvider, Task> func);

    /// <summary>
    ///     Executes the provided operation on the web application fixture's service provider using a new asynchronous service
    ///     scope, and returns the result.
    /// </summary>
    /// <param name="func">The operation to be executed within the service scope.</param>
    /// <typeparam name="T">The operation return value type.</typeparam>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation. The task result is the operation return value.</returns>
    Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func);
}
