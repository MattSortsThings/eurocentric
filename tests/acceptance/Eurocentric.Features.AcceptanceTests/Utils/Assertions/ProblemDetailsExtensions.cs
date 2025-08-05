using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;

namespace Eurocentric.Features.AcceptanceTests.Utils.Assertions;

public static class ProblemDetailsExtensions
{
    public static InvokableValueAssertionBuilder<ProblemDetails> HasTitle(this IValueSource<ProblemDetails> valueSource,
        string expected,
        [CallerArgumentExpression(nameof(expected))]
        string a1 = "") => valueSource.RegisterAssertion(new ProblemDetailsTitleEqualsExpectedValueAssertion(expected), [a1]);

    public static InvokableValueAssertionBuilder<ProblemDetails> HasDetail(this IValueSource<ProblemDetails> valueSource,
        string expected,
        [CallerArgumentExpression(nameof(expected))]
        string a1 = "") => valueSource.RegisterAssertion(new ProblemDetailsDetailEqualsExpectedValueAssertion(expected), [a1]);

    public static InvokableValueAssertionBuilder<ProblemDetails> HasStatus(this IValueSource<ProblemDetails> valueSource,
        int expected,
        [CallerArgumentExpression(nameof(expected))]
        string a1 = "") => valueSource.RegisterAssertion(new ProblemDetailsStatusEqualsExpectedValueAssertion(expected), [a1]);

    public static InvokableValueAssertionBuilder<ProblemDetails> HasExtension(this IValueSource<ProblemDetails> valueSource,
        string expectedKey,
        Guid expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))]
        string a1 = "",
        [CallerArgumentExpression(nameof(expectedKey))]
        string a2 = "") =>
        valueSource.RegisterAssertion(
            new ProblemDetailsExtensionsContainsExpectedKeyAndGuidValueAssertion(expectedKey, expectedValue),
            [a1, a2]);

    public static InvokableValueAssertionBuilder<ProblemDetails> HasExtension(this IValueSource<ProblemDetails> valueSource,
        string expectedKey,
        string expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))]
        string a1 = "",
        [CallerArgumentExpression(nameof(expectedKey))]
        string a2 = "") =>
        valueSource.RegisterAssertion(
            new ProblemDetailsExtensionsContainsExpectedKeyAndStringValueAssertion(expectedKey, expectedValue),
            [a1, a2]);

    private sealed class ProblemDetailsStatusEqualsExpectedValueAssertion(int expected)
        : ExpectedValueAssertCondition<ProblemDetails, int>(expected)
    {
        protected override string GetExpectation() => $"to be equal to {ExpectedValue}";

        protected override ValueTask<AssertionResult> GetResult(ProblemDetails? actualValue, int expectedValue) =>
            actualValue is null
                ? AssertionResult.Fail("it was null")
                : AssertionResult.FailIf(actualValue.Status != expectedValue, $"found {actualValue.Status}");
    }

    private sealed class ProblemDetailsTitleEqualsExpectedValueAssertion(string expected)
        : ExpectedValueAssertCondition<ProblemDetails, string>(expected)
    {
        protected override string GetExpectation() => $"to be equal to \"{ExpectedValue}\"";

        protected override ValueTask<AssertionResult> GetResult(ProblemDetails? actualValue, string? expectedValue) =>
            actualValue is null
                ? AssertionResult.Fail("it was null")
                : AssertionResult.FailIf(!string.Equals(actualValue.Title, expectedValue, StringComparison.InvariantCulture),
                    $"found \"{actualValue.Title}\"");
    }

    private sealed class ProblemDetailsDetailEqualsExpectedValueAssertion(string expected)
        : ExpectedValueAssertCondition<ProblemDetails, string>(expected)
    {
        protected override string GetExpectation() => $"to be equal to \"{ExpectedValue}\"";

        protected override ValueTask<AssertionResult> GetResult(ProblemDetails? actualValue, string? expectedValue) =>
            actualValue is null
                ? AssertionResult.Fail("it was null")
                : AssertionResult.FailIf(!string.Equals(actualValue.Detail, expectedValue, StringComparison.InvariantCulture),
                    $"found \"{actualValue.Detail}\"");
    }

    private class ProblemDetailsExtensionsContainsExpectedKeyAndGuidValueAssertion(string expectedKey, Guid expectedValue)
        : ValueAssertCondition<ProblemDetails>
    {
        protected override AssertionResult Passes(ProblemDetails? actualValue)
        {
            AssertionResult result;

            if (actualValue is not null)
            {
                if (!actualValue.Extensions.TryGetValue(expectedKey, out object? value))
                {
                    result = AssertionResult.Fail("key was not found");
                }

                else if (value is JsonElement je && je.GetGuid() is var g && g == expectedValue)
                {
                    result = AssertionResult.Passed;
                }
                else
                {
                    result = AssertionResult.Fail($"value was {value}");
                }
            }
            else
            {
                result = AssertionResult.Fail("it was null");
            }

            return result;
        }

        protected override string GetFailureMessage(ProblemDetails? actualValue) =>
            $"to contain the key \"{expectedKey}\" with value \"{expectedValue}\"";
    }

    private class ProblemDetailsExtensionsContainsExpectedKeyAndStringValueAssertion(string expectedKey, string expectedValue)
        : ValueAssertCondition<ProblemDetails>
    {
        protected override AssertionResult Passes(ProblemDetails? actualValue)
        {
            AssertionResult result;

            if (actualValue is not null)
            {
                if (!actualValue.Extensions.TryGetValue(expectedKey, out object? value))
                {
                    result = AssertionResult.Fail("key was not found");
                }

                else if (value is JsonElement je && je.GetString() is var s &&
                         string.Equals(s, expectedValue, StringComparison.InvariantCulture))
                {
                    result = AssertionResult.Passed;
                }
                else
                {
                    result = AssertionResult.Fail($"value was {value}");
                }
            }
            else
            {
                result = AssertionResult.Fail("it was null");
            }

            return result;
        }

        protected override string GetFailureMessage(ProblemDetails? actualValue) =>
            $"to contain the key \"{expectedKey}\" with value \"{expectedValue}\"";
    }
}
