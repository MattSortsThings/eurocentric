using Eurocentric.Infrastructure.DataAccess.Constants;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Eurocentric.WebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using Testcontainers.MsSql;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Features.AcceptanceTests.Utils;

/// <summary>
///     In-memory web app fixture.
/// </summary>
/// <remarks>Define a subclass of this type for every test collection.</remarks>
public sealed class WebAppFixture : WebApplicationFactory<IWebAppAssemblyLocator>,
    IAsyncLifetime,
    IWebAppFixtureBackDoor,
    IWebAppFixtureRestClient
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    /// <inheritdoc />
    public async ValueTask InitializeAsync() => await _dbContainer.StartAsync();

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();

        GC.SuppressFinalize(this);
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
    public async Task<ProblemOrResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await client.ExecuteAsync(request, cancellationToken);
        ProblemOrResponse problemOrResponse;

        if (response.IsSuccessful)
        {
            problemOrResponse = new ProblemOrResponse(response);
        }
        else
        {
            RestResponse<ProblemDetails> problem = await client.Deserialize<ProblemDetails>(response, cancellationToken);

            problemOrResponse = new ProblemOrResponse(problem);
        }

        return problemOrResponse;
    }

    /// <inheritdoc />
    public async Task<ProblemOrResponse<T>> SendAsync<T>(RestRequest request, CancellationToken cancellationToken = default)
        where T : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<T> response = await client.ExecuteAsync<T>(request, cancellationToken);
        ProblemOrResponse<T> problemOrResponse;

        if (response.IsSuccessful)
        {
            problemOrResponse = new ProblemOrResponse<T>(response);
        }
        else
        {
            RestResponse<ProblemDetails> problem = await client.Deserialize<ProblemDetails>(response, cancellationToken);

            problemOrResponse = new ProblemOrResponse<T>(problem);
        }

        return problemOrResponse;
    }

    /// <summary>
    ///     Resets the in-memory web app by erasing all records from the test database.
    /// </summary>
    public async Task ResetAsync() => await ExecuteScopedAsync(ResetDatabaseAsync);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ConfigureDbContainerConnectionString(builder);
        ConfigureTestApiKeys(builder);

        builder.ConfigureServices(services =>
        {
            InitializeDatabase(services);
            ConfigureRestClient(services);
            ConfigureDbContainerToggler(services);
        });
    }

    private void ConfigureDbContainerConnectionString(IWebHostBuilder builder)
    {
        string connectionString = _dbContainer.GetConnectionString().TrimEnd(';') + ";Connect Timeout=2;";

        builder.UseSetting($"ConnectionStrings:{DbConstants.ConnectionStringKey}", connectionString);
        builder.UseSetting("DbConnection:CommandTimeoutInSeconds", "1");
        builder.UseSetting("DbConnection:MaxRetryCount", "0");
    }

    private void ConfigureRestClient(IServiceCollection services) => services.AddSingleton<IRestClient>(serviceProvider =>
    {
        IOptions<HttpJsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<HttpJsonOptions>>();

        HttpClient httpClient = CreateClient();

        return new RestClient(httpClient,
            configureRestClient: options => options.Timeout = TimeSpan.FromSeconds(10),
            configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
    });

    private void ConfigureDbContainerToggler(IServiceCollection services)
    {
        DbContainerToggler toggler = new(_dbContainer);

        services.AddSingleton(toggler);
    }

    private static void ConfigureTestApiKeys(IWebHostBuilder builder)
    {
        builder.UseSetting("ApiKeySecurity:DemoApiKey", TestApiKeys.DemoApiKey);
        builder.UseSetting("ApiKeySecurity:SecretApiKey", TestApiKeys.SecretApiKey);
    }

    private static void InitializeDatabase(IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
    }

    private static async Task ResetDatabaseAsync(IServiceProvider provider)
    {
        await using AppDbContext dbContext = provider.GetRequiredService<AppDbContext>();

        await dbContext.Broadcasts.ExecuteDeleteAsync();
        await dbContext.Contests.ExecuteDeleteAsync();
        await dbContext.Countries.ExecuteDeleteAsync();
        await dbContext.PlaceholderContests.ExecuteDeleteAsync();
        await dbContext.PlaceholderQueryableCountries.ExecuteDeleteAsync();
        await dbContext.PlaceholderQueryableJuryAwards.ExecuteDeleteAsync();
        await dbContext.PlaceholderQueryableTelevoteAwards.ExecuteDeleteAsync();
    }
}
