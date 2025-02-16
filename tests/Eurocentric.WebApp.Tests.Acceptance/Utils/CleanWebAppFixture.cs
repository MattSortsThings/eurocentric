using Eurocentric.WebApp.Tests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services =>
    {
        services.AddSingleton<IRestClient>(serviceProvider =>
        {
            IOptions<JsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<JsonOptions>>();

            HttpClient httpClient = CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = BaseUri, AllowAutoRedirect = false
            });

            return new RestClient(httpClient,
                configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
        });
    });

    internal void Reset() => ExecuteScoped(_ => { });

    internal async Task<RestResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default) =>
        await ExecuteScopedAsync(serviceProvider =>
        {
            IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();

            return restClient.ExecuteAsync<T>(request, cancellationToken);
        });

    internal async Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default) =>
        await ExecuteScopedAsync(serviceProvider =>
        {
            IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();

            return restClient.ExecuteAsync(request, cancellationToken);
        });
}
