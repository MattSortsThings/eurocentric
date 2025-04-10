using Eurocentric.WebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using Testcontainers.MsSql;
using Xunit;

namespace Eurocentric.TestUtils.WebAppFixtures;

public sealed class WebAppFixture : WebApplicationFactory<IWebAppAssemblyLocator>, IAsyncLifetime, ITestHttpClient
{
    private static readonly Uri BaseAddress = new("http://localhost:5263");

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    public async ValueTask InitializeAsync() => await _dbContainer.StartAsync();

    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public async Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await restClient.ExecuteAsync(request, cancellationToken);
    }

    public async Task<RestResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await restClient.ExecuteAsync<T>(request, cancellationToken);
    }

    public void Reset() { }

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
                configureRestClient: options => options.Timeout = TimeSpan.FromSeconds(10),
                configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
        });
    });
}
