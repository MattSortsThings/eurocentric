using Microsoft.AspNetCore.Mvc;
using RestSharp;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public sealed class ProblemOrResponseIsProblemAssertion : Assertion<RestResponse<ProblemDetails>>
{
    public ProblemOrResponseIsProblemAssertion(AssertionContext<ProblemOrResponse> context)
        : base(context.Map<RestResponse<ProblemDetails>>(r => r!.AsProblem)) { }

    protected override string GetExpectation() => "to be Problem";

    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<RestResponse<ProblemDetails>> metadata)
    {
        AssertionResult result = metadata.Exception is not null
            ? AssertionResult.Failed(metadata.Exception.Message)
            : AssertionResult.Passed;

        return Task.FromResult(result);
    }
}
