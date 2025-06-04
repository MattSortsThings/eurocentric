using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utilities;

public static class RestRequestExtensions
{
    public static RestRequest AddQueryParameters(this RestRequest request, IReadOnlyDictionary<string, object> queryParams)
    {
        foreach (var (key, value) in queryParams.OrderBy(kvp => kvp.Key))
        {
            switch (value)
            {
                case string s:
                    request.AddQueryParameter(key, s);

                    break;
                case int i:
                    request.AddQueryParameter(key, i);

                    break;
                case bool b:
                    request.AddQueryParameter(key, b);

                    break;
                default:
                    throw new NotSupportedException($"Query parameter type {value.GetType()} is not supported.");
            }
        }

        return request;
    }

    public static RestRequest UseDemoApiKey(this RestRequest request) => request.AddHeader("X-Api-Key", TestApiKeys.Demo);

    public static RestRequest UseSecretApiKey(this RestRequest request) => request.AddHeader("X-Api-Key", TestApiKeys.Secret);
}
