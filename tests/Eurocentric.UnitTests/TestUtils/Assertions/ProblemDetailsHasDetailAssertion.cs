using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.UnitTests.TestUtils.Assertions;

public sealed class ProblemDetailsHasDetailAssertion : Assertion<ProblemDetails>
{
    private readonly string _expected;

    public ProblemDetailsHasDetailAssertion(AssertionContext<ProblemDetails> context, string expected)
        : base(context)
    {
        _expected = expected;
    }

    protected override string GetExpectation() => $"to have Detail \"{_expected}\"";

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
        else if (value.Detail is { } actual && !string.Equals(actual, _expected, StringComparison.Ordinal))
        {
            result = AssertionResult.Failed($"Detail was \"{actual}\"");
        }

        return Task.FromResult(result);
    }
}
