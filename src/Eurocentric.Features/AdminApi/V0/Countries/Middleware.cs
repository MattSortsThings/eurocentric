using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Versioning;
using Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;
using Eurocentric.Features.AdminApi.V0.Countries.GetCountry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V0.Countries;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the "Countries" tagged endpoints to the endpoint route builder.
    /// </summary>
    /// <param name="builder">The endpoint route builder.</param>
    internal static void MapCountriesEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder endpointGroup = builder.MapGroup("countries")
            .WithTags(EndpointNames.Countries.Tag)
            .WithDescription("Operations on the Country resource.");

        endpointGroup.MapPost("/", CreateCountryFeature.ExecuteAsync)
            .WithName(EndpointNames.Countries.CreateCountry)
            .WithSummary("Create a country")
            .WithDescription("Creates a new country in the system.")
            .IntroducedInVersion0Point(1)
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        endpointGroup.MapGet("/{countryId:guid}", GetCountryFeature.ExecuteAsync)
            .WithName(EndpointNames.Countries.GetCountry)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country from the system.")
            .IntroducedInVersion0Point(1)
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
