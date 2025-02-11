using Eurocentric.AdminApi.V0.Calculations.Common;
using MediatR;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal sealed class CreateCalculationHandler : IRequestHandler<CreateCalculationCommand, CreateCalculationResponse>
{
    public Task<CreateCalculationResponse> Handle(CreateCalculationCommand request, CancellationToken cancellationToken)
    {
        Func<int, int, int> func = request.Operation switch
        {
            Operation.Modulus => (x, y) => x % y,
            Operation.Product => (x, y) => x * y,
            _ => throw new InvalidOperationException("Invalid enum value")
        };

        int result = func(request.X, request.Y);

        return Task.FromResult(new CreateCalculationResponse(new Calculation(request.X, request.Y, request.Operation, result)));
    }
}
