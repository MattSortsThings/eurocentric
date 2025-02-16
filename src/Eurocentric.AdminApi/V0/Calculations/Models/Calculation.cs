namespace Eurocentric.AdminApi.V0.Calculations.Models;

public sealed record Calculation
{
    public required int X { get; init; }

    public required int Y { get; init; }

    public required Operation Operation { get; init; }

    public required int Result { get; init; }

    public required DateOnly DateRequested { get; init; }
}
