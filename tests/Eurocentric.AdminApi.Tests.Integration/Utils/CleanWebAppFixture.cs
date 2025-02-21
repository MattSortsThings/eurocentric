using Eurocentric.WebApp.Tests.Fixtures;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi.Tests.Integration.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    /// <summary>
    ///     Resets the test database to its initial state.
    /// </summary>
    public void Reset() => ExecuteScoped(_ => { });

    /// <summary>
    ///     Sends the request to the application pipeline of the web app fixture, and returns its result.
    /// </summary>
    /// <param name="request">The request to be handled.</param>
    /// <param name="cancellationToken">Cancels the operation.</param>
    /// <typeparam name="T">The response type.</typeparam>
    /// <returns>A task representing the operation, with the operation result as the task's result.</returns>
    public async Task<T> SendAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default)
    {
        return await ExecuteScopedAsync(Function);

        Task<T> Function(IServiceProvider serviceProvider)
        {
            ISender sender = serviceProvider.GetRequiredService<ISender>();

            return sender.Send(request, cancellationToken);
        }
    }
}
