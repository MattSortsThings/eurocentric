using System.Text.Json;
using System.Text.Json.Serialization;
using Eurocentric.WebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using RestSharp;
using RestSharp.Serializers.Json;
using Testcontainers.MsSql;
using Xunit;

namespace Eurocentric.TestUtils.WebAppFixtures;

public class WebAppFixture : WebApplicationFactory<IWebAppMarker>, IAsyncLifetime
{
    private static readonly Uri BaseUri = new("http://localhost:5131");
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().Build();

    private RestClient? _restClient;

    public RestClient RestClient => _restClient ??= CreateRestClient();

    public async ValueTask InitializeAsync() => await _dbContainer.StartAsync();

    public void Reset()
    {
        // Reset database here
    }

    private RestClient CreateRestClient()
    {
        HttpClient httpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri, AllowAutoRedirect = false
        });

        return new RestClient(httpClient, configureSerialization: config =>
        {
            config.UseSystemTextJson(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Converters = { new JsonStringEnumConverter() }
            });
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
