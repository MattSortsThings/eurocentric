using Eurocentric.Features.Shared.Security;
using Eurocentric.Infrastructure.EfCore;
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
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public abstract class WebAppFixtureBase : WebApplicationFactory<IWebAppAssemblyMarker>, IAsyncLifetime, ITestClient
{
    private static readonly Uri BaseUri = new("http://localhost:5205");

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithCleanUp(true).Build();

    private string DbConnectionString { get; set; } = string.Empty;

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        DbConnectionString = _dbContainer.GetConnectionString();
    }

    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public async Task<ResponseOrProblem> SendRequestAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await client.ExecuteAsync(request, cancellationToken);

        return response.IsSuccessful
            ? new ResponseOrProblem(response)
            : new ResponseOrProblem(await client.Deserialize<ProblemDetails>(response, cancellationToken));
    }

    public async Task<ResponseOrProblem<TResponse>> SendRequestAsync<TResponse>(RestRequest request,
        CancellationToken cancellationToken = default)
        where TResponse : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<TResponse> response = await client.ExecuteAsync<TResponse>(request, cancellationToken);

        return response.IsSuccessful
            ? new ResponseOrProblem<TResponse>(response)
            : new ResponseOrProblem<TResponse>(await client.Deserialize<ProblemDetails>(response, cancellationToken));
    }

    public void ExecuteScoped(Action<IServiceProvider> action)
    {
        using IServiceScope scope = Services.CreateScope();
        action(scope.ServiceProvider);
    }

    public async Task ExecuteScopedAsync(Func<IServiceProvider, Task> action)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await action(scope.ServiceProvider);
    }

    public async Task<T> ExecuteScopedAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        return await action(scope.ServiceProvider);
    }

    protected void EraseAllData()
    {
        Action<IServiceProvider> eraseAllData = sp =>
        {
            using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            dbContext.Contests.ExecuteDelete();
            dbContext.Countries.ExecuteDelete();
        };

        ExecuteScoped(eraseAllData);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.ConfigureTestServices(services =>
    {
        ConfigureAppDbContextUsingDbContainerConnectionString(services);
        MigrateDb(services);
        ConfigureTestApiKeys(services);
        ConfigureRestClient(services);
    });

    private void ConfigureRestClient(IServiceCollection services) => services.AddSingleton<IRestClient>(serviceProvider =>
    {
        IOptions<JsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<JsonOptions>>();

        HttpClient httpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri, AllowAutoRedirect = false
        });

        return new RestClient(httpClient,
            configureRestClient: options => options.Timeout = TimeSpan.FromSeconds(10),
            configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
    });

    private void ConfigureAppDbContextUsingDbContainerConnectionString(IServiceCollection services)
    {
        if (services.SingleOrDefault(serviceDescriptor =>
                serviceDescriptor.ServiceType == typeof(DbContextOptions<AppDbContext>)) is { } descriptor)
        {
            services.Remove(descriptor);
        }

        services.AddAppDbContext(DbConnectionString);
    }

    private static void ConfigureTestApiKeys(IServiceCollection services) =>
        services.Configure<ApiKeySecurityOptions>(options =>
        {
            options.SecretApiKey = TestApiKeys.Secret;
            options.DemoApiKey = TestApiKeys.Demo;
        });

    private static void MigrateDb(IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}
