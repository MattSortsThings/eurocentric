namespace Eurocentric.Features.AcceptanceTests.PublicApi.V1.Utils.Comparers;

internal static class DoubleExtensions
{
    private const double Tolerance = 0.000001d;

    internal static bool EquivalentTo6dp(this double a, double b) => Math.Abs(a - b) < Tolerance;
}
