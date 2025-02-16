using Eurocentric.WebApp.Tests.Fixtures;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.PublicApi.Tests.Integration.Utils;

public sealed class SeededWebAppFixture : WebAppFixture
{
    internal async Task<T> SendAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default) =>
        await ExecuteScopedAsync(serviceProvider =>
        {
            ISender sender = serviceProvider.GetRequiredService<ISender>();

            return sender.Send(request, cancellationToken);
        });
}
