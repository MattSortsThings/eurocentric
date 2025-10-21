using TUnit.Assertions.Core;

namespace Eurocentric.AcceptanceTests.TestUtils.Assertions;

public static class ProblemOrResponseExtensions
{
    public static ProblemOrResponseIsProblemAssertion IsProblem(this IAssertionSource<ProblemOrResponse> source)
    {
        source.Context.ExpressionBuilder.Append(".IsProblem()");

        return new ProblemOrResponseIsProblemAssertion(source.Context);
    }

    public static ProblemOrResponseIsResponseAssertion IsResponse(this IAssertionSource<ProblemOrResponse> source)
    {
        source.Context.ExpressionBuilder.Append(".IsResponse()");

        return new ProblemOrResponseIsResponseAssertion(source.Context);
    }
}
