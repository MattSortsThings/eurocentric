namespace Eurocentric.Features.AcceptanceTests.Utils;

/// <summary>
///     Allows the client to manipulate the web app fixture's state using a delegate invoked on its service provider.
/// </summary>
public interface IWebAppFixtureBackDoor
{
    /// <summary>
    ///     Executes the provided operation on the web app fixture's service provider using a new synchronous service scope.
    /// </summary>
    /// <param name="action">
    ///     Encapsulates a synchronous operation that takes an application service provider as its input and
    ///     does not return a value. The operation to be executed.
    /// </param>
    public void ExecuteScoped(Action<IServiceProvider> action);

    /// <summary>
    ///     Executes the provided operation on the web app fixture's service provider using a new asynchronous service scope.
    /// </summary>
    /// <param name="func">
    ///     Encapsulates an asynchronous operation that takes an application service provider as its input and
    ///     does not return a value. The operation to be executed.
    /// </param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task ExecuteScopedAsync(Func<IServiceProvider, Task> func);
}
