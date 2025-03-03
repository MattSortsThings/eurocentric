using System.ComponentModel;
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

        int result = command.Operation switch
        {
            Operation.Product => command.X * command.Y,
            Operation.Modulus => command.X % command.Y,
            _ => throw new InvalidEnumArgumentException("operation", (int)command.Operation, typeof(Operation))
        };

        return new CreateCalculationResult(new Calculation(Guid.NewGuid(), command.X, command.Y, command.Operation, result));
    }
}
