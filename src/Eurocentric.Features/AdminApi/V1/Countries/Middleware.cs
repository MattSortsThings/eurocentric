using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Versioning;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountries;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1.Countries;

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
            .WithTags(Endpoints.Countries.Tag)
            .WithDescription("Operations on the Country resource.");

        endpointGroup.MapPost("/", CreateCountryFeature.ExecuteAsync)
            .WithName(Endpoints.Countries.CreateCountry)
            .WithSummary("Create a country")
            .WithDescription("Creates a new country in the system.")
            .IntroducedInVersion1Point0()
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        endpointGroup.MapGet("/", GetCountriesFeature.ExecuteAsync)
            .WithName(Endpoints.Countries.GetCountries)
            .WithSummary("Get all countries")
            .WithDescription("Retrieves all existing countries from the system, ordered by country code.")
            .IntroducedInVersion1Point0()
            .Produces<GetCountriesResponse>();

        endpointGroup.MapGet("/{countryId:guid}", GetCountryFeature.ExecuteAsync)
            .WithName(Endpoints.Countries.GetCountry)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country from the system.")
            .IntroducedInVersion1Point0()
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
