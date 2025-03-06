using Eurocentric.AdminApi.V1.Models;
using Eurocentric.Shared.AppPipeline;

namespace Eurocentric.AdminApi.V1.Countries.CreateCountry;

public sealed record CreateCountryCommand : Command<CreateCountryResult>
{
    public required string CountryCode { get; init; }

    public required string CountryName { get; init; }

    public required CountryType CountryType { get; init; }
}
