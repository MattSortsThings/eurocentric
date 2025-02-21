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
    /// <summary>
    ///     Resets the test database to its initial state.
    /// </summary>
    public void Reset() => ExecuteScoped(_ => { });

    /// <summary>
    ///     Sends the request to the web app fixture and returns its response.
    /// </summary>
    /// <param name="request">The request to be sent.</param>
    /// <param name="cancellationToken">Cancels the operation.</param>
    /// <typeparam name="T">The expected response object type.</typeparam>
    /// <returns>A completed task containing the response.</returns>
    public async Task<RestResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
    {
        return await ExecuteScopedAsync(Function);

        Task<RestResponse<T>> Function(IServiceProvider serviceProvider)
        {
            IRestClient client = serviceProvider.GetRequiredService<IRestClient>();

            return client.ExecuteAsync<T>(request, cancellationToken);
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services =>
    {
        services.AddSingleton<IRestClient>(serviceProvider =>
        {
            IOptions<JsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<JsonOptions>>();

            HttpClient httpClient = CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = BaseAddress, AllowAutoRedirect = false
            });

            return new RestClient(httpClient,
                configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
        });
    });
}
