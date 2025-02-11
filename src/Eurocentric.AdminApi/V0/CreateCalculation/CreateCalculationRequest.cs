using Eurocentric.AdminApi.V0.Calculations.Common;

namespace Eurocentric.AdminApi.V0.CreateCalculation;

public sealed record CreateCalculationRequest
{
    public int X { get; init; }

    public int Y { get; init; }

    public Operation Operation { get; init; }
}
