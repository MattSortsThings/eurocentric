using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.Features.AdminApi.V1.Countries.DeleteCountry;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountries;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1.Countries;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the endpoints tagged with "Countries".
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapCountriesEndpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("countries")
            .WithTags(EndpointNames.Tags.Countries);

        group.MapPost("/", CreateCountryFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Countries.CreateCountry)
            .WithSummary("Create a country")
            .WithDescription("Creates a new country.")
            .HasApiVersion(1, 0)
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        group.MapDelete("/{countryId:guid}", DeleteCountryFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Countries.DeleteCountry)
            .WithSummary("Delete a country")
            .WithDescription("Deletes a single country.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapGet("/{countryId:guid}", GetCountryFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Countries.GetCountry)
            .WithSummary("Get a country")
            .WithDescription("Retrieves a single country.")
            .HasApiVersion(1, 0)
            .Produces<GetCountryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/", GetCountriesFeature.ExecuteAsync)
            .WithName(EndpointNames.Routes.Countries.GetCountries)
            .WithSummary("Get all countries")
            .WithDescription("Retrieves a list of all existing countries, in country code order.")
            .HasApiVersion(1, 0)
            .Produces<GetCountriesResponse>();
    }
}
