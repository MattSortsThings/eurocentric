namespace Eurocentric.AdminApi.V0.Calculations.Models;

public record Calculation(Guid Id, int X, int Y, Operation Operation, int Output);
