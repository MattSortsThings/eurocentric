using Eurocentric.Features.Shared.Security;
using Eurocentric.Infrastructure;
using Eurocentric.Infrastructure.EFCore;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Eurocentric.WebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using Testcontainers.MsSql;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

/// <summary>
///     In-memory web application fixture with a test database running in a Docker container.
/// </summary>
public sealed class WebAppFixture : WebApplicationFactory<IWebAppAssemblyLocator>,
    IAsyncLifetime,
    IWebAppFixtureBackDoor,
    IWebAppFixtureRestClient
{
    private static readonly Uri BaseUri = new("http://localhost:5206");

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    private string DbConnectionString { get; set; } = string.Empty;

    /// <inheritdoc />
    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        DbConnectionString = _dbContainer.GetConnectionString();
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
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
    public async Task<ProblemOrResponse> SendRequestAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await client.ExecuteAsync(request, cancellationToken);

        return response.IsSuccessStatusCode
            ? new ProblemOrResponse(response)
            : new ProblemOrResponse(await client.Deserialize<ProblemDetails>(response, cancellationToken));
    }

    /// <inheritdoc />
    public async Task<ProblemOrResponse<T>> SendRequestAsync<T>(RestRequest request,
        CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<T> response = await client.ExecuteAsync<T>(request, cancellationToken);

        return response.IsSuccessStatusCode
            ? new ProblemOrResponse<T>(response)
            : new ProblemOrResponse<T>(await client.Deserialize<ProblemDetails>(response, cancellationToken));
    }

    /// <summary>
    ///     Resets the web application fixture to its original empty state.
    /// </summary>
    public void Reset()
    {
        using IServiceScope scope = Services.CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        InMemoryContestRepository contestRepo = scope.ServiceProvider.GetRequiredService<InMemoryContestRepository>();
        InMemoryQueryableRepository queryableRepo = scope.ServiceProvider.GetRequiredService<InMemoryQueryableRepository>();

        contestRepo.Reset();
        queryableRepo.Reset();
        dbContext.Broadcasts.ExecuteDelete();
        dbContext.Contests.ExecuteDelete();
        dbContext.Countries.ExecuteDelete();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services =>
    {
        ConfigureAppDbContextUsingDbContainerConnectionString(services);
        ConfigureRestClient(services);
        ConfigureTestApiKeys(services);
        MigrateDb(services);
    });

    private void ConfigureRestClient(IServiceCollection services) => services.AddSingleton<IRestClient>(serviceProvider =>
    {
        IOptions<HttpJsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<HttpJsonOptions>>();

        HttpClient httpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri, AllowAutoRedirect = false
        });

        return new RestClient(httpClient,
            configureRestClient: options => options.Timeout = TimeSpan.FromSeconds(10),
            configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
    });

    private static void ConfigureTestApiKeys(IServiceCollection services) =>
        services.Configure<ApiKeySecurityOptions>(options =>
        {
            options.SecretApiKey = TestApiKeys.Secret;
            options.DemoApiKey = TestApiKeys.Demo;
        });

    private void ConfigureAppDbContextUsingDbContainerConnectionString(IServiceCollection services)
    {
        if (services.SingleOrDefault(serviceDescriptor =>
                serviceDescriptor.ServiceType == typeof(DbContextOptions<AppDbContext>)) is { } descriptor)
        {
            services.Remove(descriptor);
        }

        services.AddEfCoreAppDbContext(DbConnectionString);
    }

    private static void MigrateDb(IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}
