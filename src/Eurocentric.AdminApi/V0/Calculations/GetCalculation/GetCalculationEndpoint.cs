using Asp.Versioning;
using ErrorOr;
using Eurocentric.AdminApi.Common;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.Shared.ApiRegistration;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.AdminApi.V0.Calculations.GetCalculation;

internal sealed record GetCalculationEndpoint : IEndpointInfo
{
    internal const string Name = "GetCalculation";

    public Delegate Handler => async ([FromRoute(Name = "calculationId")] Guid calculationId,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetCalculationResult> errorsOrResult =
            await sender.Send(new GetCalculationQuery(calculationId), cancellationToken);

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public string Resource => "calculations/{calculationId:guid}";

    public HttpMethod Method => HttpMethod.Get;

    public string EndpointId => Name;

    public ApiVersion InitialApiVersion => AdminApiInfo.Versions.V0.Point1;

    public string Tag => AdminApiInfo.Tags.Calculations;

    public string Summary => "Get calculation";

    public string Description => "Retrieves a single calculation.";

    public IEnumerable<int> ProblemStatusCodes => AdminApiInfo.UniversalProblemStatusCodes.Append(StatusCodes.Status404NotFound);

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new GetCalculationResult(new Calculation(Guid.NewGuid(), 5, 10, Operation.Product, 50));
        }
    }
}
