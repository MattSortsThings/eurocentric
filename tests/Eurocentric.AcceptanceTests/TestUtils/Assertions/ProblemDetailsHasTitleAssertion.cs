using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public sealed class ProblemDetailsHasTitleAssertion : Assertion<ProblemDetails>
{
    private readonly string _expected;

    public ProblemDetailsHasTitleAssertion(AssertionContext<ProblemDetails> context, string expected)
        : base(context)
    {
        _expected = expected;
    }

    protected override string GetExpectation() => $"to have Title \"{_expected}\"";

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
        else if (value.Title is { } actual && !string.Equals(actual, _expected, StringComparison.Ordinal))
        {
            result = AssertionResult.Failed($"Title was \"{actual}\"");
        }

        return Task.FromResult(result);
    }
}
