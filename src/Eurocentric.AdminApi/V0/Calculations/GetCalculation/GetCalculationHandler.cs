using ErrorOr;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V0.Calculations.GetCalculation;

internal sealed class GetCalculationHandler : QueryHandler<GetCalculationQuery, GetCalculationResult>
{
    public override async Task<ErrorOr<GetCalculationResult>> Handle(GetCalculationQuery query,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return query.CalculationId.ToString().StartsWith("00000000")
            ? Error.NotFound("Calculation not found",
                "No calculation exists with the specified ID.",
                new Dictionary<string, object> { ["calculationId"] = query.CalculationId })
            : new GetCalculationResult(new Calculation(query.CalculationId, 5, 10, Operation.Product, 50));
    }
}
