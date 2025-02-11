using Eurocentric.AdminApi.V0.Calculations.Common;
using MediatR;

namespace Eurocentric.AdminApi.V0.CreateCalculation;

public record CreateCalculationCommand(int X, int Y, Operation Operation) : IRequest<CreateCalculationResponse>;
