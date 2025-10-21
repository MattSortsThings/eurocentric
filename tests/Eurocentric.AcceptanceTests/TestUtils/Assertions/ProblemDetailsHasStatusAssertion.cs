using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public sealed class ProblemDetailsHasStatusAssertion : Assertion<ProblemDetails>
{
    private readonly int _expected;

    public ProblemDetailsHasStatusAssertion(AssertionContext<ProblemDetails> context, int expected)
        : base(context)
    {
        _expected = expected;
    }

    protected override string GetExpectation() => $"to have Status {_expected}";

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
        else if (value.Status is { } actual && actual != _expected)
        {
            result = AssertionResult.Failed($"Status was {actual}");
        }

        return Task.FromResult(result);
    }
}
