using Eurocentric.Components.DataAccess.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using TUnit.Core.Interfaces;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Tests.Acceptance.Utils;

/// <summary>
///     An in-memory web application that creates, uses and deletes its own uniquely-named test database.
/// </summary>
/// <remarks>
///     The web application creates its test database when its <see cref="InitializeAsync" /> method is invoked. It
///     deletes its test database when its <see cref="DisposeAsync" /> method is invoked.
/// </remarks>
public sealed class WebAppFixture : WebApplicationFactory<Program>, IAsyncInitializer, IWebAppFixture
{
    private readonly string _testDbName = "db_" + Guid.NewGuid().ToString("N");

    /// <summary>
    ///     A shared Microsoft SQL Server instance running inside a container.
    /// </summary>
    [ClassDataSource<DbContainerFixture>(Shared = SharedType.PerTestSession)]
    public required DbContainerFixture DbContainer { get; init; }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        _ = Server;

        await MigrateTestDbAsync();
    }

    /// <inheritdoc />
    public async Task EraseAllDataAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.V0Broadcasts.ExecuteDeleteAsync();
        await dbContext.V0Contests.ExecuteDeleteAsync();
        await dbContext.V0Countries.ExecuteDeleteAsync();
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
    public async Task<UnionRestResponse> SendRequestAsync(RestRequest request)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await restClient.ExecuteAsync(
            request,
            TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None
        );

        return response.IsSuccessful
            ? response
            : await restClient.Deserialize<ProblemDetails>(response, CancellationToken.None);
    }

    /// <inheritdoc />
    public async Task<UnionRestResponse<T>> SendRequestAsync<T>(RestRequest request)
        where T : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<T> response = await restClient.ExecuteAsync<T>(
            request,
            TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None
        );

        return response.IsSuccessful
            ? response
            : await restClient.Deserialize<ProblemDetails>(response, CancellationToken.None);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync() => await EnsureTestDbDeletedAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string dbConnectionString = DbContainer.GetConnectionString(_testDbName);

        builder.UseSetting("AzureSqlDb:ConnectionString", dbConnectionString);
        builder.UseSetting("AzureSqlDb:CommandTimeoutInSeconds", "5");
        builder.UseSetting("AzureSqlDb:MaxRetries", "0");

        builder.ConfigureServices(AddRestClient);
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

    private async Task MigrateTestDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    private async Task EnsureTestDbDeletedAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }
}
