using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils;

/// <summary>
///     Exposes methods for interacting with an in-memory web application.
/// </summary>
public interface IWebAppFixture
{
    /// <summary>
    ///     Asynchronously erases all data in the database.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous erase.</returns>
    Task EraseAllDataAsync();

    /// <summary>
    ///     Asynchronously executes the provided operation on the web application's service provider using a new asynchronous
    ///     service scope.
    /// </summary>
    /// <param name="func">The asynchronous operation to be executed.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous execution.</returns>
    Task ExecuteScopedAsync(Func<IServiceProvider, Task> func);

    /// <summary>
    ///     Asynchronously executes the provided operation on the web application's service provider using a new asynchronous
    ///     service scope, and returns the result.
    /// </summary>
    /// <param name="func">The asynchronous operation to be executed.</param>
    /// <typeparam name="T">The return value type.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous execution. The task's result is the value returned by
    ///     the operation.
    /// </returns>
    Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func);

    /// <summary>
    ///     Asynchronously sends the provided REST request to the web application and returns the successful or unsuccessful
    ///     REST response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous send. The task's result is the REST response.
    /// </returns>
    Task<UnionRestResponse> SendRequestAsync(RestRequest request);

    /// <summary>
    ///     Asynchronously sends the provided REST request to the web application and returns the successful or unsuccessful
    ///     REST response.
    /// </summary>
    /// <param name="request">The REST request to be sent.</param>
    /// <typeparam name="T">The successful response body type.</typeparam>
    /// <returns>A <see cref="Task" /> representing the asynchronous send. The task's result is the REST response.</returns>
    Task<UnionRestResponse<T>> SendRequestAsync<T>(RestRequest request)
        where T : class;
}
