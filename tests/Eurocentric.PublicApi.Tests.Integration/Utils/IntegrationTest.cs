using MediatR;

namespace Eurocentric.PublicApi.Tests.Integration.Utils;

[Trait("Category", "DatabaseTest")]
[Trait("Category", "IntegrationTest")]
public abstract class IntegrationTest(SeededWebAppFixture fixture)
{
    /// <summary>
    ///     Sends the request to the application pipeline of the web app fixture, and returns its result.
    /// </summary>
    /// <param name="request">The request to be handled.</param>
    /// <typeparam name="T">The response type.</typeparam>
    /// <returns>A task representing the operation, with the operation result as the task's result.</returns>
    private protected async Task<T> SendAsync<T>(IRequest<T> request) =>
        await fixture.SendAsync(request, TestContext.Current.CancellationToken);
}
