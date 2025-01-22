using System.Text.Json;
using System.Text.Json.Serialization;
using Eurocentric.WebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using RestSharp;
using RestSharp.Serializers.Json;
using Testcontainers.MsSql;
using Xunit;

namespace Eurocentric.Tests.Utils.Fixtures;

public abstract class WebAppFixture : WebApplicationFactory<IWebAppAssemblyLocator>, IAsyncLifetime, ITestableWebApi
{
    private static readonly Uri BaseUri = new("http://localhost:5214");

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    private RestClient? _restClient;

    private RestClient RestClient => _restClient ??= CreateRestClient();

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await SeedDatabaseAsync();
    }

    public Task<RestResponse> ExecuteAsync(RestRequest request, CancellationToken cancellationToken = default) =>
        RestClient.ExecuteAsync(request, cancellationToken);

    public Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default) =>
        RestClient.ExecuteAsync<T>(request, cancellationToken);

    /// <summary>
    ///     Resets the web app test database to its initial state.
    /// </summary>
    public void Reset() { }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    /// <summary>
    ///     Override this method to seed the test database.
    /// </summary>
    /// <remarks>
    ///     This method is invoked exactly once, during initialization of this instance. The base implementation of the
    ///     method does nothing.
    /// </remarks>
    /// <returns>A completed task.</returns>
    protected virtual Task SeedDatabaseAsync() => Task.CompletedTask;

    private RestClient CreateRestClient()
    {
        HttpClient httpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false, BaseAddress = BaseUri
        });

        return new RestClient(httpClient,
            configureSerialization: config => config.UseSystemTextJson(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() }
            }));
    }
}
