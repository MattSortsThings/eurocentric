using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public static class ProblemDetailsExtensions
{
    public static ProblemDetailsHasDetailAssertion HasDetail(
        this IAssertionSource<ProblemDetails> source,
        string expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasDetail({expression})");

        return new ProblemDetailsHasDetailAssertion(source.Context, expected);
    }

    public static ProblemDetailsHasInstanceAssertion HasInstance(
        this IAssertionSource<ProblemDetails> source,
        string expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasInstance({expression})");

        return new ProblemDetailsHasInstanceAssertion(source.Context, expected);
    }

    public static ProblemDetailsHasStatusAssertion HasStatus(
        this IAssertionSource<ProblemDetails> source,
        int expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasStatus({expression})");

        return new ProblemDetailsHasStatusAssertion(source.Context, expected);
    }

    public static ProblemDetailsHasTitleAssertion HasTitle(
        this IAssertionSource<ProblemDetails> source,
        string expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasTitle({expression})");

        return new ProblemDetailsHasTitleAssertion(source.Context, expected);
    }

    public static ProblemDetailsHasTypeAssertion HasType(
        this IAssertionSource<ProblemDetails> source,
        string expected,
        [CallerArgumentExpression(nameof(expected))] string? expression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasType({expression})");

        return new ProblemDetailsHasTypeAssertion(source.Context, expected);
    }

    public static ProblemDetailsHasStringExtensionAssertion HasExtension(
        this IAssertionSource<ProblemDetails> source,
        string expectedKey,
        string expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasExtension({keyExpression}, {valueExpression})");

        return new ProblemDetailsHasStringExtensionAssertion(source.Context, expectedKey, expectedValue);
    }

    public static ProblemDetailsHasInt32ExtensionAssertion HasExtension(
        this IAssertionSource<ProblemDetails> source,
        string expectedKey,
        int expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasExtension({keyExpression}, {valueExpression})");

        return new ProblemDetailsHasInt32ExtensionAssertion(source.Context, expectedKey, expectedValue);
    }

    public static ProblemDetailsHasGuidExtensionAssertion HasExtension(
        this IAssertionSource<ProblemDetails> source,
        string expectedKey,
        Guid expectedValue,
        [CallerArgumentExpression(nameof(expectedKey))] string? keyExpression = null,
        [CallerArgumentExpression(nameof(expectedValue))] string? valueExpression = null
    )
    {
        source.Context.ExpressionBuilder.Append($".HasExtension({keyExpression}, {valueExpression})");

        return new ProblemDetailsHasGuidExtensionAssertion(source.Context, expectedKey, expectedValue);
    }
}
