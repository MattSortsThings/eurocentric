using Asp.Versioning;
using ErrorOr;
using Eurocentric.AdminApi.Common;
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

    public ApiVersion InitialApiVersion => ApiVersions.V0.Point1;

    public string Tag => ApiTags.Calculations;

    public string Summary => "Get calculation";

    public string Description => "Retrieves a single calculation.";
}
