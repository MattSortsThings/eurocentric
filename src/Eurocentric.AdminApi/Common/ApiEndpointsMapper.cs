using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.GetCalculation;
using Eurocentric.Shared.ApiMapping;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.Common;

internal sealed class ApiEndpointsMapper : IApiEndpointsMapper
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGetCalculationV0Point1();
        app.MapGetCalculationV0Point2();
        app.MapCreateCalculationV0Point2();
    }
}
