using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Eurocentric.AcceptanceTests.TestUtils.Fixtures;

/// <summary>
///     An in-memory test web app using a shared containerized SQL Server instance.
/// </summary>
public sealed partial class TestWebApp
{
    /// <inheritdoc />
    public async Task<SuccessOrFailureRestResponse> SendAsync(RestRequest request)
    {
        SuccessOrFailureRestResponse output;
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await restClient
            .ExecuteAsync(request, TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsSuccessful)
        {
            output = SuccessOrFailureRestResponse.FromSuccessRestResponse(response);
        }
        else
        {
            RestResponse<ProblemDetails> failureResponse = await restClient
                .Deserialize<ProblemDetails>(response, CancellationToken.None)
                .ConfigureAwait(false);

            output = SuccessOrFailureRestResponse.FromFailureRestResponse(failureResponse);
        }

        return output;
    }

    /// <inheritdoc />
    public async Task<SuccessOrFailureRestResponse<TBody>> SendAsync<TBody>(RestRequest request)
        where TBody : class
    {
        SuccessOrFailureRestResponse<TBody> output;
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<TBody> response = await restClient
            .ExecuteAsync<TBody>(request, TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None)
            .ConfigureAwait(false);

        if (response.IsSuccessful)
        {
            output = SuccessOrFailureRestResponse<TBody>.FromSuccessRestResponse(response);
        }
        else
        {
            RestResponse<ProblemDetails> failureResponse = await restClient
                .Deserialize<ProblemDetails>(response, CancellationToken.None)
                .ConfigureAwait(false);

            output = SuccessOrFailureRestResponse<TBody>.FromFailureRestResponse(failureResponse);
        }

        return output;
    }

    /// <inheritdoc />
    public async Task SendSafeAsync(RestRequest request)
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse response = await restClient
            .ExecuteAsync(request, TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None)
            .ConfigureAwait(false);

        await Assert.That(response.IsSuccessful).IsTrue();
    }

    /// <inheritdoc />
    public async Task<TBody> SendSafeAsync<TBody>(RestRequest request)
        where TBody : class
    {
        await using AsyncServiceScope scope = Services.CreateAsyncScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        RestResponse<TBody> response = await restClient
            .ExecuteAsync<TBody>(request, TestContext.Current?.Execution.CancellationToken ?? CancellationToken.None)
            .ConfigureAwait(false);

        await Assert.That(response.IsSuccessful).IsTrue();

        return await Assert.That(response.Data).IsNotNull();
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
