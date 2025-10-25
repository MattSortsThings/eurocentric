using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Features.Countries;
using Eurocentric.Components.DataAccess.EfCore;
using Eurocentric.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using CountryDto = Eurocentric.Apis.Admin.V1.Dtos.Countries.Country;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;

public static partial class Shortcuts
{
    public static async Task<CountryDto> CreateACountryAsync(
        this AdminKernel kernel,
        string countryName = "",
        string countryCode = ""
    )
    {
        RestRequest request = kernel.Requests.Countries.CreateCountry(
            new CreateCountryRequest { CountryCode = countryCode, CountryName = countryName }
        );

        ProblemOrResponse<CreateCountryResponse> response = await kernel.Client.SendAsync<CreateCountryResponse>(
            request
        );

        return response.AsResponse.Data!.Country;
    }

    public static async Task DeleteACountryAsync(this AdminKernel kernel, Guid countryId)
    {
        CountryId id = CountryId.FromValue(countryId);
        await kernel.BackDoor.ExecuteScopedAsync(DeleteAsync(id));
    }

    public static async Task<CountryDto[]> GetAllCountriesAsync(this AdminKernel kernel)
    {
        RestRequest request = kernel.Requests.Countries.GetCountries();

        ProblemOrResponse<GetCountriesResponse> response = await kernel.Client.SendAsync<GetCountriesResponse>(request);

        return response.AsResponse.Data!.Countries;
    }

    private static Func<IServiceProvider, Task> DeleteAsync(CountryId countryId)
    {
        CountryId idToDelete = countryId;

        return async sp =>
        {
            await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
            await dbContext.Countries.Where(country => country.Id.Equals(idToDelete)).ExecuteDeleteAsync();
        };
    }
}
