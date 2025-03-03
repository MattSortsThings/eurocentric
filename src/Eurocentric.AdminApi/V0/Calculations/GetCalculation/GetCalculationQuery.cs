using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V0.Calculations.GetCalculation;

public sealed record GetCalculationQuery(Guid CalculationId) : Query<GetCalculationResult>;
