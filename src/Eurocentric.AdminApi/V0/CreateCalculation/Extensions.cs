namespace Eurocentric.AdminApi.V0.CreateCalculation;

internal static class Extensions
{
    internal static CreateCalculationCommand ToCommand(this CreateCalculationRequest request) =>
        new(request.X, request.Y, request.Operation);
}
