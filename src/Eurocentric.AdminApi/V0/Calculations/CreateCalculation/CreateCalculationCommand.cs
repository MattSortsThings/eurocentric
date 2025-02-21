using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

public sealed record CreateCalculationCommand : Command<CreateCalculationResult>
{
    public required int X { get; init; }

    public required int Y { get; init; }

    public required Operation Operation { get; init; }

    internal void Deconstruct(out int x, out int y, out Operation operation)
    {
        x = X;
        y = Y;
        operation = Operation;
    }
}
