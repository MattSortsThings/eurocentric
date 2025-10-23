using Microsoft.AspNetCore.Mvc;
using RestSharp;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils;

public static class ProblemOrResponseAssertions
{
    public static IsProblemAssertion IsProblem(this IAssertionSource<ProblemOrResponse> source)
    {
        source.Context.ExpressionBuilder.Append(".IsProblem()");

        return new IsProblemAssertion(source.Context);
    }

    public static IsResponseAssertion IsResponse(this IAssertionSource<ProblemOrResponse> source)
    {
        source.Context.ExpressionBuilder.Append(".IsResponse()");

        return new IsResponseAssertion(source.Context);
    }

    public sealed class IsProblemAssertion : Assertion<RestResponse<ProblemDetails>>
    {
        public IsProblemAssertion(AssertionContext<ProblemOrResponse> context)
            : base(context.Map<RestResponse<ProblemDetails>>(problemOrResponse => problemOrResponse?.AsProblem)) { }

        protected override string GetExpectation() => "to be Problem";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<RestResponse<ProblemDetails>> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed(exception.Message);
            }

            if (metadata.Value is null)
            {
                result = AssertionResult.Failed("value was null");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class IsResponseAssertion : Assertion<RestResponse>
    {
        public IsResponseAssertion(AssertionContext<ProblemOrResponse> context)
            : base(context.Map<RestResponse>(problemOrResponse => problemOrResponse?.AsResponse)) { }

        protected override string GetExpectation() => "to be Response";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<RestResponse> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed(exception.Message);
            }

            if (metadata.Value is null)
            {
                result = AssertionResult.Failed("value was null");
            }

            return Task.FromResult(result);
        }
    }
}
