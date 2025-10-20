using Eurocentric.Components.DataAccess.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using TUnit.Core.Interfaces;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Abstract base class for an in-memory web application that uses a SQL Server database running inside a test
///     container.
/// </summary>
public abstract class WebAppFixture
    : WebApplicationFactory<Program>,
        IAsyncInitializer,
        IWebAppFixtureBackDoor,
        IWebAppFixtureRestClient
{
    [ClassDataSource<DbContainerFixture>(Shared = SharedType.PerClass)]
    public required DbContainerFixture DbContainer { get; init; }

    /// <summary>
    ///     Asynchronously initializes the fixture by starting the server, then applying migrations to the database, then
    ///     seeding the database as specified in the concrete derivative.
    /// </summary>
    public async Task InitializeAsync()
    {
        _ = Server;
        await MigrateDbAsync();
        await SeedDbAsync();
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

        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await restClient.SendAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProblemOrResponse<T>> SendAsync<T>(
        RestRequest request,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await restClient.SendAsync<T>(request, cancellationToken);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(
            (_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        { "AzureSqlDb:CommandTimeoutInSeconds", "5" },
                        { "AzureSqlDb:ConnectionString", DbContainer.ConnectionString },
                        { "AzureSqlDb:MaxRetries", "0" },
                        { "AzureSqlDb:HttpRetryAfterSeconds", "120" },
                    }
                );
            }
        );

        builder.ConfigureTestServices(AddRestClient);
    }

    /// <summary>
    ///     Asynchronously seeds the database.
    /// </summary>
    /// <remarks>Implement this method in concrete derivative to seed data in the database during initialization.</remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous seeding operation.</returns>
    private protected abstract Task SeedDbAsync();

    private async Task MigrateDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    private void AddRestClient(IServiceCollection services) =>
        services.AddSingleton<IRestClient>(serviceProvider =>
        {
            IOptions<HttpJsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<HttpJsonOptions>>();

            HttpClient httpClient = CreateClient();

            return new RestClient(
                httpClient,
                configureRestClient: options => options.Timeout = TimeSpan.FromMinutes(5),
                configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions)
            );
        });
}
