using System.Net;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DomainCountry = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_country_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        admin.Given_I_want_to_retrieve_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_country_should_be_my_country();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_retrieve_non_existent_country_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_deleted_my_country();
        admin.Given_I_want_to_retrieve_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.NotFound);
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Country not found",
            detail:"No country exists with the provided country ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_country_ID_with_key("countryId");
    }

    private sealed class AdminActor : ActorWithResponse<GetCountryResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private Country? MyCountry { get; set; }

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

            MyCountry = new Country
            {
                Id = country.Id.Value, CountryCode = countryCode, CountryName = countryName, ParticipatingContests = []
            };
        }

        public void Given_I_want_to_retrieve_my_country_by_its_ID()
        {
            Assert.NotNull(MyCountry);

            Guid countryId = MyCountry.Id;
            SendMyRequest = apiDriver => apiDriver.Countries.GetCountry(countryId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_country_should_be_my_country()
        {
            Assert.NotNull(MyCountry);
            Assert.NotNull(ResponseObject);

            Assert.Equal(MyCountry, ResponseObject.Country, new CountryEqualityComparer());
        }

        public async Task Given_I_have_deleted_my_country()
        {
            Assert.NotNull(MyCountry);

            var targetId = CountryId.FromValue(MyCountry.Id);

            Func<IServiceProvider, Task> delete = async sp =>
            {
                await using var dbContext = sp.GetRequiredService<AppDbContext>();
                await dbContext.Countries.Where(country => country.Id == targetId).ExecuteDeleteAsync();
            };

            await BackDoor.ExecuteScopedAsync(delete);
        }

        public void Then_the_problem_details_extensions_should_contain_my_country_ID_with_key(string key)
        {
            Assert.NotNull(MyCountry);

            Then_the_problem_details_extensions_should_contain(key, MyCountry.Id);
        }
    }
}
