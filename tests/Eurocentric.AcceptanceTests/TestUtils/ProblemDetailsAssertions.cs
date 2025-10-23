using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils;

public static class ProblemDetailsAssertions
{
    public static HasDetailAssertion HasDetail(
        this IAssertionSource<ProblemDetails> source,
        string expectedDetail,
        [CallerArgumentExpression(nameof(expectedDetail))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasDetail({expression})");

        return new HasDetailAssertion(source.Context, expectedDetail);
    }

    public static HasGuidExtensionAssertion HasExtension(
        this IAssertionSource<ProblemDetails> source,
        string expectedKey,
        Guid expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasExtension({keyExpression}, {valueExpression})");

        return new HasGuidExtensionAssertion(source.Context, expectedKey, expectedValue);
    }

    public static HasInt32ExtensionAssertion HasExtension(
        this IAssertionSource<ProblemDetails> source,
        string expectedKey,
        int expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasExtension({keyExpression}, {valueExpression})");

        return new HasInt32ExtensionAssertion(source.Context, expectedKey, expectedValue);
    }

    public static HasStringExtensionAssertion HasExtension(
        this IAssertionSource<ProblemDetails> source,
        string expectedKey,
        string expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasExtension({keyExpression}, {valueExpression})");

        return new HasStringExtensionAssertion(source.Context, expectedKey, expectedValue);
    }

    public static HasInstanceAssertion HasInstance(
        this IAssertionSource<ProblemDetails> source,
        string expectedInstance,
        [CallerArgumentExpression(nameof(expectedInstance))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasInstance({expression})");

        return new HasInstanceAssertion(source.Context, expectedInstance);
    }

    public static HasStatusAssertion HasStatus(
        this IAssertionSource<ProblemDetails> source,
        int expectedStatus,
        [CallerArgumentExpression(nameof(expectedStatus))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasStatus({expression})");

        return new HasStatusAssertion(source.Context, expectedStatus);
    }

    public static HasTitleAssertion HasTitle(
        this IAssertionSource<ProblemDetails> source,
        string expectedTitle,
        [CallerArgumentExpression(nameof(expectedTitle))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasTitle({expression})");

        return new HasTitleAssertion(source.Context, expectedTitle);
    }

    public static HasTypeAssertion HasType(
        this IAssertionSource<ProblemDetails> source,
        string expectedType,
        [CallerArgumentExpression(nameof(expectedType))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($"HasType({expression})");

        return new HasTypeAssertion(source.Context, expectedType);
    }

    public sealed class HasDetailAssertion : Assertion<ProblemDetails>
    {
        private readonly string _expectedDetail;

        public HasDetailAssertion(AssertionContext<ProblemDetails> context, string expectedDetail)
            : base(context)
        {
            _expectedDetail = expectedDetail;
        }

        protected override string GetExpectation() => $"to have Detail \"{_expectedDetail}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<ProblemDetails> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Detail is not { } actualDetail)
            {
                result = AssertionResult.Failed("Detail was null");
            }
            else if (!string.Equals(actualDetail, _expectedDetail, StringComparison.Ordinal))
            {
                result = AssertionResult.Failed($"Detail was \"{actualDetail}\"");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasGuidExtensionAssertion : Assertion<ProblemDetails>
    {
        private readonly string _expectedKey;
        private readonly Guid _expectedValue;

        public HasGuidExtensionAssertion(
            AssertionContext<ProblemDetails> context,
            string expectedKey,
            Guid expectedValue
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
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Extensions is not { } actualExtensions)
            {
                result = AssertionResult.Failed("Extensions was null");
            }
            else if (!actualExtensions.TryGetValue(_expectedKey, out object? actualValue))
            {
                result = AssertionResult.Failed("key was not present");
            }
            else if (actualValue is not JsonElement je || !je.TryGetGuid(out Guid actualGuidValue))
            {
                result = AssertionResult.Failed($"value was \"{actualValue}\"");
            }
            else if (actualGuidValue != _expectedValue)
            {
                result = AssertionResult.Failed($"value was {actualGuidValue}");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasInt32ExtensionAssertion : Assertion<ProblemDetails>
    {
        private readonly string _expectedKey;
        private readonly int _expectedValue;

        public HasInt32ExtensionAssertion(
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
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Extensions is not { } actualExtensions)
            {
                result = AssertionResult.Failed("Extensions was null");
            }
            else if (!actualExtensions.TryGetValue(_expectedKey, out object? actualValue))
            {
                result = AssertionResult.Failed("key was not present");
            }
            else if (actualValue is not JsonElement je || !je.TryGetInt32(out int actualIntValue))
            {
                result = AssertionResult.Failed($"value was \"{actualValue}\"");
            }
            else if (actualIntValue != _expectedValue)
            {
                result = AssertionResult.Failed($"value was {actualIntValue}");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasStringExtensionAssertion : Assertion<ProblemDetails>
    {
        private readonly string _expectedKey;
        private readonly string _expectedValue;

        public HasStringExtensionAssertion(
            AssertionContext<ProblemDetails> context,
            string expectedKey,
            string expectedValue
        )
            : base(context)
        {
            _expectedKey = expectedKey;
            _expectedValue = expectedValue;
        }

        protected override string GetExpectation() => $"to have Extension \"{_expectedKey}\": \"{_expectedValue}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<ProblemDetails> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Extensions is not { } actualExtensions)
            {
                result = AssertionResult.Failed("Extensions was null");
            }
            else if (!actualExtensions.TryGetValue(_expectedKey, out object? actualValue))
            {
                result = AssertionResult.Failed("key was not present");
            }
            else if (actualValue is not JsonElement je || je.GetString() is not { } actualStringValue)
            {
                result = AssertionResult.Failed($"value was \"{actualValue}\"");
            }
            else if (!string.Equals(actualStringValue, _expectedValue, StringComparison.Ordinal))
            {
                result = AssertionResult.Failed($"value was \"{actualStringValue}\"");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasInstanceAssertion : Assertion<ProblemDetails>
    {
        private readonly string _expectedInstance;

        public HasInstanceAssertion(AssertionContext<ProblemDetails> context, string expectedInstance)
            : base(context)
        {
            _expectedInstance = expectedInstance;
        }

        protected override string GetExpectation() => $"to have Instance \"{_expectedInstance}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<ProblemDetails> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Instance is not { } actualInstance)
            {
                result = AssertionResult.Failed("Instance was null");
            }
            else if (!string.Equals(actualInstance, _expectedInstance, StringComparison.Ordinal))
            {
                result = AssertionResult.Failed($"Instance was \"{actualInstance}\"");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasStatusAssertion : Assertion<ProblemDetails>
    {
        private readonly int _expectedStatus;

        public HasStatusAssertion(AssertionContext<ProblemDetails> context, int expectedStatus)
            : base(context)
        {
            _expectedStatus = expectedStatus;
        }

        protected override string GetExpectation() => $"to have Status {_expectedStatus}";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<ProblemDetails> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Status is not { } actualStatus)
            {
                result = AssertionResult.Failed("Status was null");
            }
            else if (actualStatus != _expectedStatus)
            {
                result = AssertionResult.Failed($"Status was {actualStatus}");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasTitleAssertion : Assertion<ProblemDetails>
    {
        private readonly string _expectedTitle;

        public HasTitleAssertion(AssertionContext<ProblemDetails> context, string expectedTitle)
            : base(context)
        {
            _expectedTitle = expectedTitle;
        }

        protected override string GetExpectation() => $"to have Title \"{_expectedTitle}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<ProblemDetails> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Title is not { } actualTitle)
            {
                result = AssertionResult.Failed("Title was null");
            }
            else if (!string.Equals(actualTitle, _expectedTitle, StringComparison.Ordinal))
            {
                result = AssertionResult.Failed($"Title was \"{actualTitle}\"");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasTypeAssertion : Assertion<ProblemDetails>
    {
        private readonly string _expectedType;

        public HasTypeAssertion(AssertionContext<ProblemDetails> context, string expectedType)
            : base(context)
        {
            _expectedType = expectedType;
        }

        protected override string GetExpectation() => $"to have Type \"{_expectedType}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<ProblemDetails> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } problemDetails)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (problemDetails.Type is not { } actualType)
            {
                result = AssertionResult.Failed("Type was null");
            }
            else if (!string.Equals(actualType, _expectedType, StringComparison.Ordinal))
            {
                result = AssertionResult.Failed($"Type was \"{actualType}\"");
            }

            return Task.FromResult(result);
        }
    }
}
