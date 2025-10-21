using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.UnitTests.TestUtils.Assertions;

public sealed class ProblemDetailsHasEmptyExtensionsAssertion : Assertion<ProblemDetails>
{
    public ProblemDetailsHasEmptyExtensionsAssertion(AssertionContext<ProblemDetails> context)
        : base(context) { }

    protected override string GetExpectation() => "to have empty Extensions";

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
        else if (value.Extensions.Count is var actual and > 0)
        {
            result = AssertionResult.Failed($"Extensions count was {actual}");
        }

        return Task.FromResult(result);
    }
}
