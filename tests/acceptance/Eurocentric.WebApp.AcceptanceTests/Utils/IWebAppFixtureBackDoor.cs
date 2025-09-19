namespace Eurocentric.WebApp.AcceptanceTests.Utils;

/// <summary>
///     Allows the client to modify the state of the in-memory web app fixture at runtime using scoped operations on the
///     fixture's service provider.
/// </summary>
public interface IWebAppFixtureBackDoor
{
    /// <summary>
    ///     Executes the provided operation on the web app fixture's service provider using a new synchronous service scope.
    /// </summary>
    /// <param name="action">
    ///     Encapsulates a synchronous operation that takes an application service provider as its input and does not return a
    ///     value. The operation to be executed.
    /// </param>
    void ExecuteScoped(Action<IServiceProvider> action);

    /// <summary>
    ///     Executes the provided operation on the web app fixture's service provider using a new asynchronous service scope.
    /// </summary>
    /// <param name="func">
    ///     Encapsulates an asynchronous operation that takes an application service provider as its input and does not return
    ///     a value. The operation to be executed.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteScopedAsync(Func<IServiceProvider, Task> func);

    /// <summary>
    ///     Executes the provided operation on the web app fixture's service provider using a new asynchronous service scope
    ///     and returns the result.
    /// </summary>
    /// <param name="func">
    ///     Encapsulates an asynchronous operation that takes an application service provider as its input and returns a value.
    ///     The operation to be executed.
    /// </param>
    /// <returns>A task representing the asynchronous operation. The task result is the output of the operation.</returns>
    Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func);
}
