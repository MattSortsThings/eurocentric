using ErrorOr;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal sealed class CreateCalculationHandler : CommandHandler<CreateCalculationCommand, CreateCalculationResult>
{
    public override async Task<ErrorOr<CreateCalculationResult>> Handle(CreateCalculationCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var (x, y, operation) = command;

        int result = operation switch
        {
            Operation.Product => x * y,
            Operation.Modulus => x % y,
            _ => throw new InvalidOperationException("Invalid enum value")
        };

        return new CreateCalculationResult(new Calculation
        {
            Id = Guid.CreateVersion7(),
            DateRequested = DateOnly.FromDateTime(DateTime.UtcNow),
            X = x,
            Y = y,
            Operation = operation,
            Result = result
        });
    }
}
