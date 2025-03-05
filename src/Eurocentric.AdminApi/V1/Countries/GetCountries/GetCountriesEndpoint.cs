using ErrorOr;
using Eurocentric.AdminApi.Common;
using Eurocentric.AdminApi.V1.Countries.Models;
using Eurocentric.Shared.ApiAbstractions;
using Eurocentric.Shared.ErrorHandling;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.AdminApi.V1.Countries.GetCountries;

internal sealed record GetCountriesEndpoint : IEndpointInfo
{
    public string Name => nameof(GetCountries);

    public Delegate Handler => async (ISender sender, CancellationToken cancellationToken = default) =>
    {
        ErrorOr<GetCountriesResult> errorsOrResult = await sender.Send(new GetCountriesQuery(), cancellationToken);

        return errorsOrResult.ToHttpResult(TypedResults.Ok);
    };

    public HttpMethod Method => HttpMethod.Get;

    public string Route => "countries";

    public int MajorApiVersion => 1;

    public int MinorApiVersion => 0;

    public string Summary => "Get all countries";

    public string Description => "Retrieves a list of all the existing countries, ordered by country code.";

    public string Tag => EndpointTags.Countries;

    public IEnumerable<int> ProblemStatusCodes
    {
        get
        {
            yield break;
        }
    }

    public IEnumerable<object> Examples
    {
        get
        {
            yield return new GetCountriesResult(
                [
                    new Country(Guid.NewGuid(),
                        "GB",
                        "United Kingdom",
                        CountryType.Real,
                        Enumerable.Range(0, 2).Select(_ => Guid.NewGuid()).ToArray())
                ]
            );
        }
    }
}
