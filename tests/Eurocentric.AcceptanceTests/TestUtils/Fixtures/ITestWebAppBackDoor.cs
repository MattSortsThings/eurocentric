namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

/// <summary>
///     Allows the user to interact with the test web app directly, bypassing its API endpoints.
/// </summary>
public interface ITestWebAppBackDoor
{
    /// <summary>
    ///     Executes the provided function on the test web app's service provider within a new synchronous service scope.
    /// </summary>
    /// <param name="func">
    ///     A synchronous function that operates on the service provider and does not return a value.
    /// </param>
    void ExecuteScoped(Action<IServiceProvider> func);

    /// <summary>
    ///     Executes the provided function on the test web app's service provider within a new asynchronous
    ///     service scope.
    /// </summary>
    /// <param name="func">
    ///     An asynchronous function that operates on the service provider and does not return a value.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    Task ExecuteScopedAsync(Func<IServiceProvider, Task> func);

    /// <summary>
    ///     Executes the provided function on the test web app's service provider within a new asynchronous
    ///     service scope and     returns the result.
    /// </summary>
    /// <param name="func">An asynchronous function that operates on the service provider and returns a value.</param>
    /// <typeparam name="T">The function return value type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation. The task's result is the value returned
    ///     by the function.
    /// </returns>
    Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func);
}
