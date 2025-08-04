using System.Runtime.CompilerServices;
using ErrorOr;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;

namespace Eurocentric.Domain.UnitTests.Utils.Assertions;

public static class ErrorExtensions
{
    public static InvokableValueAssertionBuilder<Error> HasType(this IValueSource<Error> valueSource,
        ErrorType expected,
        [CallerArgumentExpression(nameof(expected))]
        string a1 = "") => valueSource.RegisterAssertion(
        new ErrorTypeEqualsExpectedValueAssertion(expected),
        [a1]);

    public static InvokableValueAssertionBuilder<Error> HasCode(this IValueSource<Error> valueSource,
        string expected,
        [CallerArgumentExpression(nameof(expected))]
        string a1 = "") => valueSource.RegisterAssertion(
        new ErrorCodeEqualsExpectedValueAssertion(expected),
        [a1]);

    public static InvokableValueAssertionBuilder<Error> HasDescription(this IValueSource<Error> valueSource,
        string expected,
        [CallerArgumentExpression(nameof(expected))]
        string a1 = "") => valueSource.RegisterAssertion(
        new ErrorDescriptionEqualsExpectedValueAssertion(expected),
        [a1]);

    public static InvokableValueAssertionBuilder<Error> HasMetadataEntry(this IValueSource<Error> valueSource,
        string expectedKey,
        string expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))]
        string a1 = "",
        [CallerArgumentExpression(nameof(expectedValue))]
        string a2 = "") => valueSource.RegisterAssertion(
        new ErrorMetadataContainsExpectedKeyAndStringValueAssertion(expectedKey, expectedValue),
        [a1, a2]);

    private sealed class ErrorCodeEqualsExpectedValueAssertion(string expected)
        : ExpectedValueAssertCondition<Error, string>(expected)
    {
        protected override string GetExpectation() => $"to be equal to \"{ExpectedValue}\"";

        protected override ValueTask<AssertionResult> GetResult(Error actualValue, string? expectedValue) =>
            AssertionResult.FailIf(!string.Equals(actualValue.Code, expectedValue, StringComparison.InvariantCulture),
                $"found \"{actualValue.Code}\"");
    }

    private sealed class ErrorDescriptionEqualsExpectedValueAssertion(string expected)
        : ExpectedValueAssertCondition<Error, string>(expected)
    {
        protected override string GetExpectation() => $"to be equal to \"{ExpectedValue}\"";

        protected override ValueTask<AssertionResult> GetResult(Error actualValue, string? expectedValue) =>
            AssertionResult.FailIf(!string.Equals(actualValue.Description, expectedValue, StringComparison.InvariantCulture),
                $"found \"{actualValue.Description}\"");
    }

    private class ErrorTypeEqualsExpectedValueAssertion(ErrorType expected)
        : ExpectedValueAssertCondition<Error, ErrorType>(expected)
    {
        protected override string GetExpectation() => $"to be equal to {ExpectedValue}";

        protected override ValueTask<AssertionResult> GetResult(Error actualValue, ErrorType expectedValue) =>
            AssertionResult.FailIf(actualValue.Type != expectedValue, $"found {actualValue.Type}");
    }

    private class ErrorMetadataContainsExpectedKeyAndStringValueAssertion(string expectedKey, string expectedValue)
        : ValueAssertCondition<Error>
    {
        protected override AssertionResult Passes(Error actualValue)
        {
            AssertionResult result;

            if (actualValue.Metadata is { } metadata)
            {
                if (!metadata.TryGetValue(expectedKey, out object? value))
                {
                    result = AssertionResult.Fail("key was not found");
                }

                else if (value is not string stringValue)
                {
                    result = AssertionResult.Fail($"value was {value}");
                }

                else
                {
                    result = AssertionResult.FailIf(stringValue != expectedValue, $"value was \"{stringValue}\"");
                }
            }
            else
            {
                result = AssertionResult.Fail("metadata was null");
            }

            return result;
        }

        protected override string GetFailureMessage(Error actualValue) =>
            $"to contain the key \"{expectedKey}\" with value \"{expectedValue}\"";
    }

    private class ErrorMetadataContainsExpectedKeyAndInt32ValueAssertion(string expectedKey, int expectedValue)
        : ValueAssertCondition<Error>
    {
        protected override AssertionResult Passes(Error actualValue)
        {
            AssertionResult result;

            if (actualValue.Metadata is { } metadata)
            {
                if (!metadata.TryGetValue(expectedKey, out object? value))
                {
                    result = AssertionResult.Fail("key was not found");
                }

                else if (value is not int intValue)
                {
                    result = AssertionResult.Fail($"value was {value}");
                }

                else
                {
                    result = AssertionResult.FailIf(intValue != expectedValue, $"value was {intValue}");
                }
            }
            else
            {
                result = AssertionResult.Fail("metadata was null");
            }

            return result;
        }

        protected override string GetFailureMessage(Error actualValue) =>
            $"to contain the key \"{expectedKey}\" with value {expectedValue}";
    }
}
