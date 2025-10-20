using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public static class ProblemOrResponseExtensions
{
    public static IsResponseAssertion IsResponse(this IAssertionSource<ProblemOrResponse> source)
    {
        source.Context.ExpressionBuilder.Append(".IsResponse()");

        return new IsResponseAssertion(source.Context);
    }
}
