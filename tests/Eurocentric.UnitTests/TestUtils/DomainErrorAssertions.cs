using System.Runtime.CompilerServices;
using Eurocentric.Domain.Core;
using TUnit.Assertions.Core;

namespace Eurocentric.UnitTests.TestUtils;

public static class DomainErrorAssertions
{
    public static HasNullExtensionsAssertion<T> HasNullExtensions<T>(this IAssertionSource<T> source)
        where T : IDomainError
    {
        source.Context.ExpressionBuilder.Append(".HasNullExtensions()");

        return new HasNullExtensionsAssertion<T>(source.Context);
    }

    public static HasDateOnlyExtensionAssertion<T> HasExtension<T>(
        this IAssertionSource<T> source,
        string expectedKey,
        DateOnly expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
        where T : IDomainError
    {
        source.Context.ExpressionBuilder.Append($"HasExtension({keyExpression}, {valueExpression})");

        return new HasDateOnlyExtensionAssertion<T>(source.Context, expectedKey, expectedValue);
    }

    public static HasGuidExtensionAssertion<T> HasExtension<T>(
        this IAssertionSource<T> source,
        string expectedKey,
        Guid expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
        where T : IDomainError
    {
        source.Context.ExpressionBuilder.Append($"HasExtension({keyExpression}, {valueExpression})");

        return new HasGuidExtensionAssertion<T>(source.Context, expectedKey, expectedValue);
    }

    public static HasInt32ExtensionAssertion<T> HasExtension<T>(
        this IAssertionSource<T> source,
        string expectedKey,
        int expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
        where T : IDomainError
    {
        source.Context.ExpressionBuilder.Append($"HasExtension({keyExpression}, {valueExpression})");

        return new HasInt32ExtensionAssertion<T>(source.Context, expectedKey, expectedValue);
    }

    public static HasStringExtensionAssertion<T> HasExtension<T>(
        this IAssertionSource<T> source,
        string expectedKey,
        string expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
        where T : IDomainError
    {
        source.Context.ExpressionBuilder.Append($"HasExtension({keyExpression}, {valueExpression})");

        return new HasStringExtensionAssertion<T>(source.Context, expectedKey, expectedValue);
    }

    public static HasDetailAssertion<T> HasDetail<T>(
        this IAssertionSource<T> source,
        string expectedDetail,
        [CallerArgumentExpression(nameof(expectedDetail))] string? expression = null
    )
        where T : IDomainError
    {
        source.Context.ExpressionBuilder.Append($".HasDetail({expression})");

        return new HasDetailAssertion<T>(source.Context, expectedDetail);
    }

    public static HasTitleAssertion<T> HasTitle<T>(
        this IAssertionSource<T> source,
        string expectedTitle,
        [CallerArgumentExpression(nameof(expectedTitle))] string? expression = null
    )
        where T : IDomainError
    {
        source.Context.ExpressionBuilder.Append($".HasTitle({expression})");

        return new HasTitleAssertion<T>(source.Context, expectedTitle);
    }

    public sealed class HasDetailAssertion<T> : Assertion<T>
        where T : IDomainError
    {
        private readonly string _expectedDetail;

        public HasDetailAssertion(AssertionContext<T> context, string expectedDetail)
            : base(context)
        {
            _expectedDetail = expectedDetail;
        }

        protected override string GetExpectation() => $"to have Detail \"{_expectedDetail}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } domainError)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (domainError.Detail is not { } actualDetail)
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

    public sealed class HasTitleAssertion<T> : Assertion<T>
        where T : IDomainError
    {
        private readonly string _expectedTitle;

        public HasTitleAssertion(AssertionContext<T> context, string expectedTitle)
            : base(context)
        {
            _expectedTitle = expectedTitle;
        }

        protected override string GetExpectation() => $"to have Title \"{_expectedTitle}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } domainError)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (domainError.Title is not { } actualTitle)
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

    public sealed class HasNullExtensionsAssertion<T> : Assertion<T>
        where T : IDomainError
    {
        public HasNullExtensionsAssertion(AssertionContext<T> context)
            : base(context) { }

        protected override string GetExpectation() => "to have null Extensions";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } domainError)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (domainError.Extensions is not null)
            {
                result = AssertionResult.Failed("Extensions was not null");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasDateOnlyExtensionAssertion<T> : Assertion<T>
        where T : IDomainError
    {
        private readonly string _expectedKey;
        private readonly DateOnly _expectedValue;

        public HasDateOnlyExtensionAssertion(AssertionContext<T> context, string expectedKey, DateOnly expectedValue)
            : base(context)
        {
            _expectedKey = expectedKey;
            _expectedValue = expectedValue;
        }

        protected override string GetExpectation() => $"to have Extension \"{_expectedKey}\": {_expectedValue}";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } domainError)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (domainError.Extensions is not { } actualExtensions)
            {
                result = AssertionResult.Failed("Extensions was null");
            }
            else if (!actualExtensions.TryGetValue(_expectedKey, out object? actualValue))
            {
                result = AssertionResult.Failed("key was not present");
            }
            else if (actualValue is null)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (actualValue is not DateOnly actualDateOnlyValue)
            {
                result = AssertionResult.Failed($"value type was {actualValue.GetType()}");
            }
            else if (actualDateOnlyValue != _expectedValue)
            {
                result = AssertionResult.Failed($"value was {actualDateOnlyValue}");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasGuidExtensionAssertion<T> : Assertion<T>
        where T : IDomainError
    {
        private readonly string _expectedKey;
        private readonly Guid _expectedValue;

        public HasGuidExtensionAssertion(AssertionContext<T> context, string expectedKey, Guid expectedValue)
            : base(context)
        {
            _expectedKey = expectedKey;
            _expectedValue = expectedValue;
        }

        protected override string GetExpectation() => $"to have Extension \"{_expectedKey}\": {_expectedValue}";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } domainError)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (domainError.Extensions is not { } actualExtensions)
            {
                result = AssertionResult.Failed("Extensions was null");
            }
            else if (!actualExtensions.TryGetValue(_expectedKey, out object? actualValue))
            {
                result = AssertionResult.Failed("key was not present");
            }
            else if (actualValue is null)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (actualValue is not Guid actualGuidValue)
            {
                result = AssertionResult.Failed($"value type was {actualValue.GetType()}");
            }
            else if (actualGuidValue != _expectedValue)
            {
                result = AssertionResult.Failed($"value was {actualGuidValue}");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasInt32ExtensionAssertion<T> : Assertion<T>
        where T : IDomainError
    {
        private readonly string _expectedKey;
        private readonly int _expectedValue;

        public HasInt32ExtensionAssertion(AssertionContext<T> context, string expectedKey, int expectedValue)
            : base(context)
        {
            _expectedKey = expectedKey;
            _expectedValue = expectedValue;
        }

        protected override string GetExpectation() => $"to have Extension \"{_expectedKey}\": {_expectedValue}";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } domainError)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (domainError.Extensions is not { } actualExtensions)
            {
                result = AssertionResult.Failed("Extensions was null");
            }
            else if (!actualExtensions.TryGetValue(_expectedKey, out object? actualValue))
            {
                result = AssertionResult.Failed("key was not present");
            }
            else if (actualValue is null)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (actualValue is not int actualIntValue)
            {
                result = AssertionResult.Failed($"value type was {actualValue.GetType()}");
            }
            else if (actualIntValue != _expectedValue)
            {
                result = AssertionResult.Failed($"value was {actualIntValue}");
            }

            return Task.FromResult(result);
        }
    }

    public sealed class HasStringExtensionAssertion<T> : Assertion<T>
        where T : IDomainError
    {
        private readonly string _expectedKey;
        private readonly string _expectedValue;

        public HasStringExtensionAssertion(AssertionContext<T> context, string expectedKey, string expectedValue)
            : base(context)
        {
            _expectedKey = expectedKey;
            _expectedValue = expectedValue;
        }

        protected override string GetExpectation() => $"to have Extension \"{_expectedKey}\": \"{_expectedValue}\"";

        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<T> metadata)
        {
            AssertionResult result = AssertionResult.Passed;

            if (metadata.Exception is { } exception)
            {
                result = AssertionResult.Failed($"{exception.GetType().Name} was thrown");
            }
            else if (metadata.Value is not { } domainError)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (domainError.Extensions is not { } actualExtensions)
            {
                result = AssertionResult.Failed("Extensions was null");
            }
            else if (!actualExtensions.TryGetValue(_expectedKey, out object? actualValue))
            {
                result = AssertionResult.Failed("key was not present");
            }
            else if (actualValue is null)
            {
                result = AssertionResult.Failed("value was null");
            }
            else if (actualValue is not string actualStringValue)
            {
                result = AssertionResult.Failed($"value type was {actualValue.GetType()}");
            }
            else if (!string.Equals(actualStringValue, _expectedValue, StringComparison.Ordinal))
            {
                result = AssertionResult.Failed($"value was \"{actualStringValue}\"");
            }

            return Task.FromResult(result);
        }
    }
}
