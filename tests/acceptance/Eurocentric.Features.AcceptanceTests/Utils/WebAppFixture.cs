using Eurocentric.Infrastructure.DataAccess.Common;
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
using TUnit.Core.Interfaces;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public abstract class WebAppFixture : WebApplicationFactory<IWebAppAssemblyLocator>,
    IAsyncInitializer,
    IWebAppFixtureBackDoor,
    IWebAppFixtureRestClient
{
    public Guid Id { get; } = Guid.NewGuid();

    private MsSqlContainer DbContainer { get; } = new MsSqlBuilder()
        .WithCleanUp(true)
        .Build();

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        await DbContainer.StartAsync();
        _ = Server;
        await MigrateDbAsync();
        await SeedDbAsync();
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

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        await DbContainer.StopAsync();
        await DbContainer.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public async Task EraseAllDataAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.V0Broadcasts.ExecuteDeleteAsync();
        await dbContext.V0Contests.ExecuteDeleteAsync();
        await dbContext.V0Countries.ExecuteDeleteAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Logging:LogLevel:Default", "Warning");
        AddDbContainerToConfiguration(builder);
        builder.ConfigureServices(ConfigureRestClient);
        AddExtraConfiguration(builder);
    }

    /// <summary>
    ///     Override this method to seed the database asynchronously during fixture initialization.
    /// </summary>
    /// <remarks>
    ///     This method is invoked exactly once, during the <see cref="InitializeAsync" /> method execution, after the web
    ///     host has been built. The base class method implementation does nothing.
    /// </remarks>
    /// <returns>A task representing the</returns>
    private protected virtual Task SeedDbAsync() => Task.CompletedTask;

    /// <summary>
    ///     Override this method to add extra configuration or services to the web host builder.
    /// </summary>
    /// <remarks>
    ///     This method is invoked exactly once, at the end of the <see cref="ConfigureWebHost" /> method execution,
    ///     before the web host has been built. The base class method implementation does nothing.
    /// </remarks>
    /// <param name="builder">The web host builder.</param>
    private protected virtual void AddExtraConfiguration(IWebHostBuilder builder) { }

    private async Task MigrateDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    private void AddDbContainerToConfiguration(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:" + DbConfigKeys.ConnectionStrings.AzureSql, DbContainer.GetConnectionString());
        builder.UseSetting(DbConfigKeys.DbConnection.CommandTimeoutInSeconds, "2");
        builder.UseSetting(DbConfigKeys.DbConnection.MaxRetryCount, "0");
    }

    private void ConfigureRestClient(IServiceCollection services) => services.AddSingleton<IRestClient>(serviceProvider =>
    {
        IOptions<HttpJsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<HttpJsonOptions>>();

        HttpClient httpClient = CreateClient();

        return new RestClient(httpClient,
            configureRestClient: options => options.Timeout = TimeSpan.FromSeconds(10),
            configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
    });
}
