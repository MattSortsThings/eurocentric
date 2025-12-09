using TUnit.Assertions.Core;

namespace Eurocentric.Tests.Architecture.Utils;

public static class EvaluationResultsAssertions
{
    public static AllPassedAssertion AllPassed(this IAssertionSource<IEnumerable<EvaluationResult>> source)
    {
        source.Context.ExpressionBuilder.Append(".AllPassed()");

        return new AllPassedAssertion(source.Context);
    }

    public class AllPassedAssertion : Assertion<IEnumerable<EvaluationResult>>
    {
        public AllPassedAssertion(AssertionContext<IEnumerable<EvaluationResult>> context)
            : base(context) { }

        protected override string GetExpectation() => "to all be Passed";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<IEnumerable<EvaluationResult>> metadata)
        {
            AssertionResult assertionResult = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                assertionResult = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } results)
            {
                assertionResult = AssertionResult.Failed("value was null");
            }
            else if (results.SingleOrDefault(er => !er.Passed) is { } notPassed)
            {
                assertionResult = AssertionResult.Failed(notPassed.Description);
            }

            return Task.FromResult(assertionResult);
        }
    }
}
