using System.Runtime.CompilerServices;
using RestSharp;
using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public static class RestResponseBaseExtensions
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
}
