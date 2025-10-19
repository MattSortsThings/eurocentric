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

public abstract class WebAppFixture : WebApplicationFactory<Program>, IAsyncInitializer
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

    public async Task ExecuteScopedAsync(Func<IServiceProvider, Task> func)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        await func(scope.ServiceProvider).ConfigureAwait(false);
    }

    public async Task<RestResponse> SendAsync(RestRequest request, CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient client = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await client.ExecuteAsync(request, cancellationToken);
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
