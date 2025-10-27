using System.Runtime.CompilerServices;
using Eurocentric.Domain.Core;
using TUnit.Assertions.Core;

namespace Eurocentric.UnitTests.TestUtils;

public static class StringAtomicValueObjectAssertions
{
    public static HasValueAssertion<T> HasValue<T>(
        this IAssertionSource<T> source,
        string expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
    )
        where T : StringAtomicValueObject
    {
        source.Context.ExpressionBuilder.Append($".HasValue({expression})");

        return new HasValueAssertion<T>(source.Context, expected);
    }

    public sealed class HasValueAssertion<T> : Assertion<T>
        where T : StringAtomicValueObject
    {
        private readonly string _expected;

        public HasValueAssertion(AssertionContext<T> context, string expected)
            : base(context)
        {
            _expected = expected;
        }

        protected override string GetExpectation() => $"to have value \"{_expected}\"";

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
                value.Value is string actualValue
                && !string.Equals(_expected, actualValue, StringComparison.Ordinal)
            )
            {
                result = AssertionResult.Failed($"Value was \"{actualValue}\"");
            }

            return Task.FromResult(result);
        }
    }
}
