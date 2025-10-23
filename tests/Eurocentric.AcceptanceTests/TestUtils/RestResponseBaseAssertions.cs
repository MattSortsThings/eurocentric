using System.Runtime.CompilerServices;
using RestSharp;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils;

public static class RestResponseBaseAssertions
{
    public static HasHeaderAssertion<T> HasHeader<T>(
        this IAssertionSource<T> source,
        string expectedName,
        string expectedValue,
        [CallerArgumentExpression(nameof(expectedName))] string? nameExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
        where T : RestResponseBase
    {
        source.Context.ExpressionBuilder.Append($".HasHeader({nameExpression}, {valueExpression})");

        return new HasHeaderAssertion<T>(source.Context, expectedName, expectedValue);
    }

    public sealed class HasHeaderAssertion<T> : Assertion<T>
        where T : RestResponseBase
    {
        private readonly string _expectedName;
        private readonly string _expectedValue;

        public HasHeaderAssertion(AssertionContext<T> context, string expectedName, string expectedValue)
            : base(context)
        {
            _expectedName = expectedName;
            _expectedValue = expectedValue;
        }

        protected override string GetExpectation() => $"to have Header \"{_expectedName}\": \"{_expectedValue}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } response)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (response.Headers is not { } headers)
            {
                result = AssertionResult.Failed("Headers was null");
            }
            else if (
                headers.SingleOrDefault(parameter => parameter.Name.Equals(_expectedName, StringComparison.Ordinal))
                is not { } header
            )
            {
                result = AssertionResult.Failed("no header with Name was found");
            }
            else if (!string.Equals(header.Value, _expectedValue, StringComparison.Ordinal))
            {
                result = AssertionResult.Failed($"header Value was \"{header.Value}\"");
            }

            return Task.FromResult(result);
        }
    }
}
