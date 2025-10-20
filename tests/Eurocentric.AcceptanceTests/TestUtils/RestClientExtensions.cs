using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Extension methods for the <see cref="IRestClient" /> interface.
/// </summary>
public static class RestClientExtensions
{
    /// <summary>
    ///     Asynchronously sends the provided REST request to the web application fixture and returns the response.
    /// </summary>
    /// <param name="client">The web application fixture REST client.</param>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous send operation. The task result is the REST response.</returns>
    public static async Task<ProblemOrResponse> SendAsync(
        this IRestClient client,
        RestRequest request,
        CancellationToken cancellationToken = default
    )
    {
        ProblemOrResponse result;
        RestResponse response = await client.ExecuteAsync(request, cancellationToken);

        if (response.IsSuccessful)
        {
            result = ProblemOrResponse.FromResponse(response);
        }
        else
        {
            result = ProblemOrResponse.FromProblem(
                await client.Deserialize<ProblemDetails>(response, cancellationToken)
            );
        }

        return result;
    }

    /// <summary>
    ///     Asynchronously sends the provided REST request to the web application fixture and returns the response.
    /// </summary>
    /// <param name="client">The web application fixture REST client.</param>
    /// <param name="request">The REST request to be sent.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <typeparam name="T">The successful REST response body type.</typeparam>
    /// <returns>A <see cref="Task" /> representing the asynchronous send operation. The task result is the REST response.</returns>
    public static async Task<ProblemOrResponse<T>> SendAsync<T>(
        this IRestClient client,
        RestRequest request,
        CancellationToken cancellationToken = default
    )
        where T : class
    {
        ProblemOrResponse<T> result;
        RestResponse<T> response = await client.ExecuteAsync<T>(request, cancellationToken);

        if (response.IsSuccessful)
        {
            result = ProblemOrResponse<T>.FromResponse(response);
        }
        else
        {
            result = ProblemOrResponse<T>.FromProblem(
                await client.Deserialize<ProblemDetails>(response, cancellationToken)
            );
        }

        return result;
    }
}
