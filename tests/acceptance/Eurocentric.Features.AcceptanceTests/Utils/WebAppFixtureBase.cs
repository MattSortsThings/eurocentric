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
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public abstract class WebAppFixtureBase : WebApplicationFactory<IWebAppAssemblyLocator>,
    IWebAppFixtureBackDoor,
    IWebAppFixtureRestClient
{
    private static readonly Uri BaseUri = new("http://localhost:5165");

    private protected abstract string DbConnectionString { get; }

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

    public async Task ResetAsync() => await ExecuteScopedAsync(ResetDatabaseAsync);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting($"ConnectionStrings:{DbConstants.ConnectionStringKey}", DbConnectionString);
        builder.ConfigureServices(services =>
        {
            InitializeDatabase(services);
            ConfigureRestClient(services);
        });
    }

    private static async Task ResetDatabaseAsync(IServiceProvider provider)
    {
        await using AppDbContext dbContext = provider.GetRequiredService<AppDbContext>();

        await dbContext.PlaceholderContests.ExecuteDeleteAsync();
        await dbContext.PlaceholderQueryableCountries.ExecuteDeleteAsync();
        await dbContext.PlaceholderQueryableJuryAwards.ExecuteDeleteAsync();
        await dbContext.PlaceholderQueryableTelevoteAwards.ExecuteDeleteAsync();
    }

    private static void InitializeDatabase(IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.Migrate();
    }

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
}
