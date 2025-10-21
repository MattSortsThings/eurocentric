using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.UnitTests.TestUtils.Assertions;

public sealed class ProblemDetailsHasNullInstanceAssertion : Assertion<ProblemDetails>
{
    public ProblemDetailsHasNullInstanceAssertion(AssertionContext<ProblemDetails> context)
        : base(context) { }

    protected override string GetExpectation() => "to have null Instance";

    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<ProblemDetails> metadata)
    {
        AssertionResult result = AssertionResult.Passed;

        if (metadata.Exception is { } exception)
        {
            result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
        }
        else if (metadata.Value is not { } value)
        {
            result = AssertionResult.Failed("value was null");
        }
        else if (value.Instance is { } actual)
        {
            result = AssertionResult.Failed($"Instance was \"{actual}\"");
        }

        return Task.FromResult(result);
    }
}
