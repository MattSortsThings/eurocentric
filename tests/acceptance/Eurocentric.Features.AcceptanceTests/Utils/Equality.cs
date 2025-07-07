namespace Eurocentric.Features.AcceptanceTests.Utils;

internal static class Equality
{
    internal static bool EqualsTo3DecimalPlaces(this double a, double b) => Math.Abs(a - b) <= 0.001;
}
