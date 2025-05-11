using Microsoft.AspNetCore.Mvc;
using OneOf;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.TestUtils;

public sealed class ResponseOrProblem : OneOfBase<RestResponse, RestResponse<ProblemDetails>>
{
    internal ResponseOrProblem(OneOf<RestResponse, RestResponse<ProblemDetails>> input) : base(input) { }
}

public sealed class ResponseOrProblem<TResult> : OneOfBase<RestResponse<TResult>, RestResponse<ProblemDetails>>
    where TResult : class
{
    internal ResponseOrProblem(OneOf<RestResponse<TResult>, RestResponse<ProblemDetails>> input) : base(input) { }
}
