using Asp.Versioning;
using ErrorOr;
using Eurocentric.AdminApi.Common;
using Eurocentric.AdminApi.V0.Calculations.GetCalculation;
using Eurocentric.Shared.ApiRegistration;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V0.Calculations.CreateCalculation;

internal sealed record CreateCalculationEndpoint : IEndpointInfo
{
    private const string Name = "CreateCalculation";

    public Delegate Handler => async ([FromBody] CreateCalculationCommand command,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<CreateCalculationResult> errorsOrResult = await sender.Send(command, cancellationToken);

        return errorsOrResult.ToHttpResult(result => TypedResults.CreatedAtRoute(result,
            GetCalculationEndpoint.Name,
            new RouteValueDictionary { ["calculationId"] = result.Calculation.Id }));
    };

    public string Resource => "calculations";

    public HttpMethod Method => HttpMethod.Post;

    public string EndpointId => Name;

    public ApiVersion InitialApiVersion => ApiVersions.V0.Point2;

    public string Tag => ApiTags.Calculations;

    public string Summary => "Create calculation";

    public string Description => "Creates a new calculation in the system.";
}
