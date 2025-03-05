using Eurocentric.DataAccess.EfCore;
using Eurocentric.Shared.Security;
using Eurocentric.WebApp.Tests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;

namespace Eurocentric.WebApp.Tests.Acceptance.Utils;

public sealed class CleanWebAppFixture : WebAppFixture
{
    /// <summary>
    ///     Resets the web app fixture to its initial state.
    /// </summary>
    public void Reset()
    {
        using IServiceScope scope = Services.CreateScope();
        using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Countries.ExecuteDelete();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(CreateRestClient);

            services.Configure<ApiKeysOptions>(options =>
            {
                options.AdminApiKey = TestApiKeys.Admin;
                options.PublicApiKey = TestApiKeys.Public;
            });
        });
    }

    private IRestClient CreateRestClient(IServiceProvider serviceProvider)
    {
        HttpClient httpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseAddress, AllowAutoRedirect = false
        });

        IOptions<JsonOptions> jsonOptions = serviceProvider.GetRequiredService<IOptions<JsonOptions>>();

        return new RestClient(httpClient,
            configureSerialization: config => config.UseSystemTextJson(jsonOptions.Value.SerializerOptions));
    }
}
