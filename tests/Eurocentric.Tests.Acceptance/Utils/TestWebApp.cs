using System.Data;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Tests.Acceptance.Utils;

public sealed class TestWebApp : WebApplicationFactory<Program>, ITestWebAppClient
{
    private readonly MsSqlServerFixture _dbServer;
    private readonly Action<IServiceCollection>? _extraConfiguration;
    private readonly string _testDbName;

    private TestWebApp(
        MsSqlServerFixture dbServer,
        string testId,
        Action<IServiceCollection>? extraConfiguration = null
    )
    {
        _dbServer = dbServer;
        _testDbName = CreateTestDbName(testId);
        _extraConfiguration = extraConfiguration;
    }

    public async Task<RestResponse> SendAsync(RestRequest request)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        return await restClient.ExecuteAsync(request);
    }

    public override async ValueTask DisposeAsync()
    {
        await DropTestDbAsync();
        await base.DisposeAsync();
    }

    public static ITestWebAppBuilder ConnectingTo(MsSqlServerFixture dbServer) => new Builder(dbServer);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = _dbServer.GetConnectionString(_testDbName);

        builder.ConfigureServices(services =>
        {
            AddRestClient(services);
            _extraConfiguration?.Invoke(services);
        });
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

    private async Task InitializeAsync()
    {
        EnsureServerStarted();
        await CreateTestDbAsync();
        await MigrateTestDbAsync();
    }

    private void EnsureServerStarted() => _ = Server;

    private async Task CreateTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(_dbServer.GetConnectionString());
        await sqlConnection.OpenAsync();
        CommandDefinition command = new(GetCreateTestDbSqlCommandText(), commandType: CommandType.Text);
        await sqlConnection.ExecuteAsync(command);
        await sqlConnection.CloseAsync();
    }

    private async Task MigrateTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(_dbServer.GetConnectionString(_testDbName));
        await sqlConnection.OpenAsync();

        CommandDefinition command = new(
            """
            CREATE TABLE dbo.country (
                country_id UNIQUEIDENTIFIER PRIMARY KEY,
                country_code NCHAR(2) NOT NULL UNIQUE,
                country_name NVARCHAR(200) NOT NULL
            );
            """,
            commandType: CommandType.Text
        );

        await sqlConnection.ExecuteAsync(command);
        await sqlConnection.CloseAsync();
    }

    private async Task DropTestDbAsync()
    {
        await using SqlConnection sqlConnection = new(_dbServer.GetConnectionString());
        await sqlConnection.OpenAsync();
        CommandDefinition command = new(GetDropTestDbSqlCommandText(), commandType: CommandType.Text);
        await sqlConnection.ExecuteAsync(command);
        await sqlConnection.CloseAsync();
    }

    private string GetCreateTestDbSqlCommandText() => $"CREATE DATABASE [{_testDbName}];";

    private string GetDropTestDbSqlCommandText() =>
        $"ALTER DATABASE [{_testDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{_testDbName}];";

    private static string CreateTestDbName(string testId) => "db_" + testId.Replace('-', '_');

    private sealed class Builder(MsSqlServerFixture dbServer) : ITestWebAppBuilder
    {
        private Action<IServiceCollection>? ExtraConfiguration { get; set; }

        public ITestWebAppBuilder WithExtraConfiguration(Action<IServiceCollection> configuration)
        {
            if (ExtraConfiguration is { } existingConfiguration)
            {
                ExtraConfiguration = existingConfiguration + configuration;
            }
            else
            {
                ExtraConfiguration = configuration;
            }

            return this;
        }

        public async Task<TestWebApp> InitializeAsync()
        {
            string testId = TestContext.Current?.Id ?? Guid.NewGuid().ToString();

            TestWebApp webApp = new(dbServer, testId, ExtraConfiguration);

            await webApp.InitializeAsync();

            return webApp;
        }
    }
}
