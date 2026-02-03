using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.Tests.Acceptance.Utils.Fixtures;

public sealed partial class TestWebApp
{
    /// <inheritdoc />
    public async Task<Result<RestResponse, RestResponse<ProblemDetails>>> SendRequestAsync(RestRequest request)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await restClient.ExecuteAsync(
            request,
            TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None
        );

        if (response.IsSuccessful)
        {
            return response;
        }

        return await restClient.Deserialize<ProblemDetails>(response, CancellationToken.None);
    }

    /// <inheritdoc />
    public async Task<Result<RestResponse<TBody>, RestResponse<ProblemDetails>>> SendRequestAsync<TBody>(
        RestRequest request
    )
        where TBody : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<TBody> response = await restClient.ExecuteAsync<TBody>(
            request,
            TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None
        );

        if (response.IsSuccessful)
        {
            return response;
        }

        return await restClient.Deserialize<ProblemDetails>(response, CancellationToken.None);
    }

    /// <inheritdoc />
    public async Task SendSafeRequestAsync(RestRequest request)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await restClient.ExecuteAsync(
            request,
            TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None
        );

        await Assert.That(response.IsSuccessful).IsTrue();
    }

    /// <inheritdoc />
    public async Task<TBody> SendSafeRequestAsync<TBody>(RestRequest request)
        where TBody : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();

        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<TBody> response = await restClient.ExecuteAsync<TBody>(
            request,
            TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None
        );

        await Assert.That(response.IsSuccessful).IsTrue();

        return response.Data!;
    }

    private void AddRestClient(IServiceCollection services)
    {
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
}
