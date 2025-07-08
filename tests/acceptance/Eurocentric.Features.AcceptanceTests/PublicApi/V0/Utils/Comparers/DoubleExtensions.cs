namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils.Comparers;

public static class DoubleExtensions
{
    private const double Tolerance = 0.000001D;

    public static bool EqualsTo6DecimalPlaces(this double value, double other) => Math.Abs(value - other) <= Tolerance;
}
