using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.WebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using TUnit.Core.Interfaces;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public abstract class WebAppFixture : WebApplicationFactory<IWebAppAssemblyLocator>,
    IAsyncInitializer,
    IWebAppFixtureBackDoor,
    IWebAppFixtureRestClient
{
    public abstract required DbContainerFixture DbContainerFixture { get; init; }

    public virtual async Task InitializeAsync()
    {
        EnsureServerStarted();
        await MigrateDbAsync();
    }

    /// <inheritdoc />
    public void ExecuteScoped(Action<IServiceProvider> action)
    {
        using IServiceScope scope = Services.CreateScope();
        action(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task ExecuteScopedAsync(Func<IServiceProvider, Task> func)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await func(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> func)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        return await func(scope.ServiceProvider);
    }

    /// <inheritdoc />
    public async Task<BiRestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await client.ExecuteRequestAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<BiRestResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
        where T : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await client.ExecuteRequestAsync<T>(request, cancellationToken);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ReplacedDbConnectionSettings(builder);
        ReplaceApiKeySecuritySettings(builder);
        builder.ConfigureServices(ConfigureRestClient);
    }

    private void ReplacedDbConnectionSettings(IWebHostBuilder builder) =>
        builder.UseSetting("ConnectionStrings:AzureSql", DbContainerFixture.GetConnectionString());

    private void EnsureServerStarted() => _ = Server;

    private async Task MigrateDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    private void ConfigureRestClient(IServiceCollection services) => services.AddSingleton<IRestClient>(serviceProvider =>
    {
        IOptions<HttpJsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<HttpJsonOptions>>();

        HttpClient httpClient = CreateClient();

        return new RestClient(httpClient,
            configureRestClient: options => options.Timeout = TimeSpan.FromSeconds(10),
            configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
    });

    private static void ReplaceApiKeySecuritySettings(IWebHostBuilder builder)
    {
        builder.UseSetting("ApiKeySecurity:DemoApiKey", TestApiKeys.Demo);
        builder.UseSetting("ApiKeySecurity:SecretApiKey", TestApiKeys.Secret);
    }
}
