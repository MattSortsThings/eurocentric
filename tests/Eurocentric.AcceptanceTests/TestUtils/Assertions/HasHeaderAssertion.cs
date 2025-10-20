using RestSharp;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public sealed class HasHeaderAssertion<T> : Assertion<T>
    where T : RestResponseBase
{
    private readonly string _expectedName;
    private readonly string _expectedValue;

    public HasHeaderAssertion(AssertionContext<T> context, string expectedName, string expectedValue)
        : base(context)
    {
        _expectedName = expectedName ?? throw new ArgumentNullException(nameof(expectedName));
        _expectedValue = expectedValue ?? throw new ArgumentNullException(nameof(expectedValue));
    }

    protected override string GetExpectation() => $"to have a Header \"{_expectedName}\": \"{_expectedValue}\"";

    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
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
        else if (
            value.Headers?.SingleOrDefault(parameter => parameter.Name.Equals(_expectedName, StringComparison.Ordinal))
            is not { } header
        )
        {
            result = AssertionResult.Failed("header name was not present");
        }
        else if (header.Value is { } headerValue && !headerValue.Equals(_expectedValue, StringComparison.Ordinal))
        {
            result = AssertionResult.Failed($"header value was \"{headerValue}\"");
        }

        return Task.FromResult(result);
    }
}
