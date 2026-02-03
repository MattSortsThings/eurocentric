using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.Utils.Contracts;

public interface ITestWebAppRestClient
{
    Task<Result<RestResponse, RestResponse<ProblemDetails>>> SendRequestAsync(RestRequest request);

    Task<Result<RestResponse<TBody>, RestResponse<ProblemDetails>>> SendRequestAsync<TBody>(RestRequest request)
        where TBody : class;

    Task SendSafeRequestAsync(RestRequest request);

    Task<TBody> SendSafeRequestAsync<TBody>(RestRequest request)
        where TBody : class;
}
