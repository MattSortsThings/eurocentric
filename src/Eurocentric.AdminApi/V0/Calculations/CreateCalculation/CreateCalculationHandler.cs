using ErrorOr;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal sealed class CreateCalculationHandler(TimeProvider timeProvider)
    : CommandHandler<CreateCalculationCommand, CreateCalculationResult>
{
    public override async Task<ErrorOr<CreateCalculationResult>> Handle(CreateCalculationCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        (int x, int y, Operation operation) = command;

        int output = operation switch
        {
            Operation.Product => x * y,
            Operation.Modulus => x % y,
            _ => throw new InvalidOperationException()
        };

        return new CreateCalculationResult(new Calculation(Guid.CreateVersion7(timeProvider.GetUtcNow()),
            x,
            y,
            operation, output));
    }
}
