using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public static class RestClientExtensions
{
    public static async Task<BiRestResponse> ExecuteRequestAsync(this IRestClient client,
        RestRequest request,
        CancellationToken cancellationToken = default)
    {
        RestResponse response = await client.ExecuteAsync(request, cancellationToken);

        if (response.IsSuccessful)
        {
            return new BiRestResponse(response);
        }

        RestResponse<ProblemDetails> problemDetails = await client.Deserialize<ProblemDetails>(response, cancellationToken);

        return new BiRestResponse(problemDetails);
    }

    public static async Task<BiRestResponse<T>> ExecuteRequestAsync<T>(this IRestClient client,
        RestRequest request,
        CancellationToken cancellationToken = default) where T : class
    {
        RestResponse<T> response = await client.ExecuteAsync<T>(request, cancellationToken);

        if (response.IsSuccessful)
        {
            return new BiRestResponse<T>(response);
        }

        RestResponse<ProblemDetails> problemDetails = await client.Deserialize<ProblemDetails>(response, cancellationToken);

        return new BiRestResponse<T>(problemDetails);
    }
}
