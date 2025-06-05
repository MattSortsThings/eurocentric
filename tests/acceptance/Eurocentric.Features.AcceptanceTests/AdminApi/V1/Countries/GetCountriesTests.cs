using System.Net;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.Extensions.DependencyInjection;
using DomainCountry = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountriesTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_all_existing_countries_in_country_code_order(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_created_a_country(countryCode: "AT", countryName: "Austria");
        await admin.Given_I_have_created_a_country(countryCode: "XX", countryName: "Rest of the World");
        await admin.Given_I_have_created_a_country(countryCode: "CH", countryName: "Switzerland");
        await admin.Given_I_have_created_a_country(countryCode: "BA", countryName: "Bosnia & Herzegovina");
        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_countries_should_be_my_countries_in_country_code_order();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_all_empty_list_when_no_countries_exist(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_countries_should_be_an_empty_list();
    }

    private sealed class AdminActor : ActorWithResponse<GetCountriesResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private List<Country> MyCountries { get; } = new(5);

        private IWebAppFixtureBackDoor BackDoor { get; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "")
        {
            DomainCountry country = new(CountryId.Create(DateTimeOffset.Now),
                CountryCode.FromValue(countryCode).Value,
                CountryName.FromValue(countryName).Value);

            Func<IServiceProvider, Task> add = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Countries.Add(country);
                await dbContext.SaveChangesAsync();
            };

            await BackDoor.ExecuteScopedAsync(add);

            Country myCountryDto = new()
            {
                Id = country.Id.Value, CountryCode = countryCode, CountryName = countryName, ParticipatingContests = []
            };

            MyCountries.Add(myCountryDto);
        }

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            SendMyRequest = apiDriver => apiDriver.Countries.GetCountries(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_countries_should_be_my_countries_in_country_code_order()
        {
            Assert.NotNull(ResponseObject);

            IOrderedEnumerable<Country> expectedCountries = MyCountries.OrderBy(country => country.CountryCode);

            Assert.Equal(expectedCountries, ResponseObject.Countries, new CountryEqualityComparer());
        }

        public void Then_the_retrieved_countries_should_be_an_empty_list()
        {
            Assert.NotNull(ResponseObject);

            Assert.Empty(ResponseObject.Countries);
        }
    }
}
