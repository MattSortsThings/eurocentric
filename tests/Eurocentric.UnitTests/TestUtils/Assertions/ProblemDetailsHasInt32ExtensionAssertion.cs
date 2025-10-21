using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.UnitTests.TestUtils.Assertions;

public sealed class ProblemDetailsHasInt32ExtensionAssertion : Assertion<ProblemDetails>
{
    private readonly string _expectedKey;
    private readonly int _expectedValue;

    public ProblemDetailsHasInt32ExtensionAssertion(
        AssertionContext<ProblemDetails> context,
        string expectedKey,
        int expectedValue
    )
        : base(context)
    {
        _expectedKey = expectedKey;
        _expectedValue = expectedValue;
    }

    protected override string GetExpectation() => $"to have Extension \"{_expectedKey}\": {_expectedValue}";

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
        else if (!value.Extensions.TryGetValue(_expectedKey, out object? actualValue))
        {
            result = AssertionResult.Failed("key was not found");
        }
        else if (actualValue is not int intValue)
        {
            result = AssertionResult.Failed("value was null");
        }
        else if (intValue != _expectedValue)
        {
            result = AssertionResult.Failed($"value was {intValue}");
        }

        return Task.FromResult(result);
    }
}
