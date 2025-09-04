using Eurocentric.Domain.ValueObjects;
using CountryAggregate = Eurocentric.Domain.Aggregates.Countries.Country;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils.Helpers.Countries;

public static class ApiDriverExtensions
{
    public static async Task<CountryDto> CreateSingleCountryAsync(
        this IApiDriver apiDriver,
        string countryName = "",
        string countryCode = "")
    {
        CountryAggregate country = new(CountryId.FromValue(Guid.NewGuid()),
            CountryCode.FromValue(countryCode).Value,
            CountryName.FromValue(countryName).Value);

        await apiDriver.BackDoor.ExecuteScopedAsync(BackDoorOperations.PersistCountryAsync(country));

        return new CountryDto
        {
            Id = country.Id.Value,
            CountryCode = country.CountryCode.Value,
            CountryName = country.CountryName.Value,
            ParticipatingContestIds = country.ParticipatingContestIds.Select(contestId => contestId.Value).ToArray()
        };
    }

    public static async Task DeleteSingleCountryAsync(this IApiDriver apiDriver, Guid countryId)
    {
        CountryId countryIdToDelete = CountryId.FromValue(countryId);

        await apiDriver.BackDoor.ExecuteScopedAsync(BackDoorOperations.DeleteCountryAsync(countryIdToDelete));
    }
}
