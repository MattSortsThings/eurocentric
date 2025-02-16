using Eurocentric.WebApp.Tests.Fixtures;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.AdminApi.Tests.Integration.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    internal void Reset() => ExecuteScoped(_ => { });

    internal async Task<T> SendAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default) =>
        await ExecuteScopedAsync(serviceProvider =>
        {
            ISender sender = serviceProvider.GetRequiredService<ISender>();

            return sender.Send(request, cancellationToken);
        });
}
