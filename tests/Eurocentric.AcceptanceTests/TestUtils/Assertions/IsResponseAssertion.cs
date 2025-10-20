using RestSharp;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public sealed class IsResponseAssertion : Assertion<RestResponse>
{
    public IsResponseAssertion(AssertionContext<ProblemOrResponse> context)
        : base(context.Map<RestResponse>(r => r!.AsResponse)) { }

    protected override string GetExpectation() => "to be Response";

    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<RestResponse> metadata)
    {
        AssertionResult result = metadata.Exception is not null
            ? AssertionResult.Failed(metadata.Exception.Message)
            : AssertionResult.Passed;

        return Task.FromResult(result);
    }
}
