using Eurocentric.Features.Shared.Security;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Utils;

public static class RestRequestExtensions
{
    public static RestRequest UseDemoApiKey(this RestRequest request)
    {
        request.AddHeader(ApiKeyConstants.HttpRequestHeaderName, TestApiKeys.Demo);

        return request;
    }

    public static RestRequest UseSecretApiKey(this RestRequest request)
    {
        request.AddHeader(ApiKeyConstants.HttpRequestHeaderName, TestApiKeys.Secret);

        return request;
    }

    public static RestRequest AddQueryParameters(this RestRequest request, IReadOnlyDictionary<string, object?> queryParams)
    {
        foreach ((string key, object? value) in queryParams)
        {
            switch (value)
            {
                case null:
                    break;
                case string s:
                    request.AddQueryParameter(key, s);

                    break;
                case int i:
                    request.AddQueryParameter(key, i);

                    break;
                case bool b:
                    request.AddQueryParameter(key, b);

                    break;
                case Guid g:
                    request.AddQueryParameter(key, g);

                    break;
                case Enum en:
                    request.AddQueryParameter(key, en.ToString());

                    break;
                default:
                    throw new NotSupportedException($"Value type {value.GetType()} is not supported.");
            }
        }

        return request;
    }
}
