using ErrorOr;
using Eurocentric.AdminApi.Common;
using Eurocentric.AdminApi.V1.Models;
using Eurocentric.Shared.ApiAbstractions;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.AdminApi.V1.Countries.CreateCountry;

internal sealed record CreateCountryEndpoint : IEndpointInfo
{
    public string Name => nameof(CreateCountry);

    public Delegate Handler => async ([FromBody] CreateCountryCommand command,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<CreateCountryResult> errorsOrResult = await sender.Send(command, cancellationToken);

        return errorsOrResult.ToHttpResult(MapToCreatedAtRoute);
    };

    public HttpMethod Method => HttpMethod.Post;

    public string Route => "countries";

    public int MajorApiVersion => 1;

    public int MinorApiVersion => 0;

    public string Summary => "Create a country";

    public string Description => "Creates a new country from parameters specified in the request body.";

    public string Tag => EndpointTags.Countries;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status400BadRequest;
            yield return StatusCodes.Status409Conflict;
            yield return StatusCodes.Status422UnprocessableEntity;
        }
    }

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new CreateCountryCommand
            {
                CountryCode = "GB", CountryName = "United Kingdom", CountryType = CountryType.Real
            };

            yield return new CreateCountryResult(new Country(Guid.NewGuid(),
                "GB",
                "United Kingdom",
                CountryType.Real,
                Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray()));
        }
    }

    private static CreatedAtRoute<CreateCountryResult> MapToCreatedAtRoute(CreateCountryResult result) =>
        TypedResults.CreatedAtRoute(result,
            nameof(GetCountry),
            new RouteValueDictionary { ["countryId"] = result.Country.Id });
}
