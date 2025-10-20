using Eurocentric.Apis.Public.V0.Enums;
using RestSharp;

namespace Eurocentric.AcceptanceTests.TestUtils;

/// <summary>
///     Extension methods for the <see cref="RestRequest" /> class.
/// </summary>
public static class RestRequestExtensions
{
    /// <summary>
    ///     Adds a query parameter to the REST request for every key-value pair with a non-<see langword="null" /> value in the
    ///     provided dictionary.
    /// </summary>
    /// <param name="request">The REST request to be modified.</param>
    /// <param name="queryParameters">A dictionary of query parameter key-value pairs.</param>
    /// <returns>The same <see cref="RestRequest" /> instance, so that method invocations can be chained.</returns>
    /// <exception cref="InvalidOperationException">
    ///     <paramref name="queryParameters" /> contains a key-value pair with an
    ///     unsupported value type.
    /// </exception>
    public static RestRequest AddQueryParameters(this RestRequest request, IDictionary<string, object?> queryParameters)
    {
        foreach ((string key, object? value) in queryParameters)
        {
            switch (value)
            {
                case null:
                    continue;
                case int intValue:
                    request.AddQueryParameter(key, intValue);

                    break;
                case bool boolValue:
                    request.AddQueryParameter(key, boolValue);

                    break;
                case string stringValue:
                    request.AddQueryParameter(key, stringValue);

                    break;
                case ContestStage csValue:
                    request.AddQueryParameter(key, csValue.ToString());

                    break;
                case ContestStageFilter csfValue:
                    request.AddQueryParameter(key, csfValue.ToString());

                    break;
                case VotingMethodFilter vmfValue:
                    request.AddQueryParameter(key, vmfValue.ToString());

                    break;
                default:
                    throw new InvalidOperationException($"Unsupported query parameter value type: {value.GetType()}.");
            }
        }

        return request;
    }
}
