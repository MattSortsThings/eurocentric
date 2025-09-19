using System.Reflection;
using Eurocentric.Infrastructure.DataAccess.EfCore;
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

namespace Eurocentric.WebApp.AcceptanceTests.Utils;

public abstract class WebAppFixture : WebApplicationFactory<IWebAppAssemblyMarker>,
    IAsyncInitializer,
    IWebAppFixtureBackDoor,
    IWebAppFixtureRestClient
{
    public async Task InitializeAsync()
    {
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

    public async Task<BiRestResponse> SendAsync(
        RestRequest request,
        CancellationToken cancellationToken = default)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        BiRestResponse biResponse;

        RestResponse response = await restClient.ExecuteAsync(request, cancellationToken);

        if (response.IsSuccessful)
        {
            biResponse = BiRestResponse.Successful(response);
        }
        else
        {
            RestResponse<ProblemDetails> problem = await restClient.Deserialize<ProblemDetails>(response, cancellationToken);

            return BiRestResponse.Unsuccessful(problem);
        }

        return biResponse;
    }

    public async Task<BiRestResponse<TResponse>> SendAsync<TResponse>(
        RestRequest request,
        CancellationToken cancellationToken = default) where TResponse : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        BiRestResponse<TResponse> biResponse;

        RestResponse<TResponse> response = await restClient.ExecuteAsync<TResponse>(request, cancellationToken);

        if (response.IsSuccessful)
        {
            biResponse = BiRestResponse<TResponse>.Successful(response);
        }
        else
        {
            RestResponse<ProblemDetails> problem = await restClient.Deserialize<ProblemDetails>(response, cancellationToken);

            return BiRestResponse<TResponse>.Unsuccessful(problem);
        }

        return biResponse;
    }

    private protected abstract string GetDbConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ModifyDbConnectionSettings(builder);
        ModifyLoggingSettings(builder);
        builder.ConfigureServices(ConfigureRestClient);
    }

    private protected virtual Task SeedDbAsync() => Task.CompletedTask;

    private async Task MigrateDbAsync()
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    private void ModifyDbConnectionSettings(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:AzureSql", GetDbConnectionString());
        builder.UseSetting("AzureDbConnection:CommandTimeoutInSeconds", "2");
        builder.UseSetting("AzureDbConnection:MinRetryCount", "0");
    }

    private void ConfigureRestClient(IServiceCollection services) => services.AddSingleton<IRestClient>(serviceProvider =>
    {
        IOptions<HttpJsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<HttpJsonOptions>>();

        HttpClient httpClient = CreateClient();

        return new RestClient(httpClient,
            configureRestClient: options => options.Timeout = TimeSpan.FromSeconds(2),
            configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
    });

    private static void ModifyLoggingSettings(IWebHostBuilder builder)
    {
        builder.UseSetting("Logging:LogLevel:Default", "None");
        builder.UseSetting("Logging:Microsoft:AspNetCore", "None");
    }

    private protected static string ReadSqlFromEmbeddedResource(string resourcePath)
    {
        Assembly assembly = typeof(WebAppFixture).Assembly;

        using Stream stream = assembly.GetManifestResourceStream(resourcePath)
                              ?? throw new InvalidOperationException($"Embedded resource '{resourcePath}' not found.");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
