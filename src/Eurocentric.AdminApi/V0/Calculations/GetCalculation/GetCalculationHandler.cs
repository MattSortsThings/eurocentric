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

        if (query.CalculationId.ToString().StartsWith('0'))
        {
            return Error.NotFound("Calculation not found",
                "No calculation exists with the specified ID.",
                new Dictionary<string, object> { ["calculationId"] = query.CalculationId });
        }

        return new GetCalculationResult(new Calculation(query.CalculationId, 5, 10, Operation.Product, 50));
    }
}
