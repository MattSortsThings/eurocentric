using System.Runtime.CompilerServices;
using Eurocentric.Domain.Core;
using TUnit.Assertions.Core;

namespace Eurocentric.UnitTests.TestUtils;

public static class DateOnlyAtomicValueObjectAssertions
{
    public static HasValueAssertion<T> HasValue<T>(
        this IAssertionSource<T> source,
        DateOnly expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
    )
        where T : DateOnlyAtomicValueObject
    {
        source.Context.ExpressionBuilder.Append($".HasValue({expression})");

        return new HasValueAssertion<T>(source.Context, expected);
    }

    public sealed class HasValueAssertion<T> : Assertion<T>
        where T : DateOnlyAtomicValueObject
    {
        private readonly DateOnly _expected;

        public HasValueAssertion(AssertionContext<T> context, DateOnly expected)
            : base(context)
        {
            _expected = expected;
        }

        protected override string GetExpectation() => $"to have value {_expected:yyyy-MM-dd}";

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
            else if (value.Value is var actualValue && actualValue != _expected)
            {
                result = AssertionResult.Failed($"Value was {actualValue}");
            }

            return Task.FromResult(result);
        }
    }
}
