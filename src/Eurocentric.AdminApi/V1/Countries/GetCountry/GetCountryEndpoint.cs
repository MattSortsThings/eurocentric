using ErrorOr;
using Eurocentric.AdminApi.Common;
using Eurocentric.AdminApi.V1.Models;
using Eurocentric.Shared.ApiAbstractions;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.AdminApi.V1.Countries.GetCountry;

internal sealed record GetCountryEndpoint : IEndpointInfo
{
    public string Name => nameof(GetCountry);

    public Delegate Handler => async ([FromRoute(Name = "countryId")] Guid countryId,
        ISender sender,
        CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetCountryResult> errorsOrResult = await sender.Send(MapToQuery(countryId), cancellationToken);

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public HttpMethod Method => HttpMethod.Get;

    public string Route => "countries/{countryId:guid}";

    public int MajorApiVersion => 1;

    public int MinorApiVersion => 0;

    public string Summary => "Get a country";

    public string Description => "Retrieves a single country." +
                                 " The client must supply the country ID as a route parameter.";

    public string Tag => EndpointTags.Countries;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield return StatusCodes.Status404NotFound;
        }
    }

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new GetCountryResult(new Country(Guid.NewGuid(),
                "GB",
                "United Kingdom",
                CountryType.Real,
                Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray()));
        }
    }

    private static GetCountryQuery MapToQuery(Guid countryId) => new(countryId);
}
