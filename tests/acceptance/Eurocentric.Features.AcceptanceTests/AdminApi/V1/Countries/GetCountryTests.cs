using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_country_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        admin.Given_I_want_to_retrieve_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_200_OK();
        admin.Then_the_retrieved_country_should_be_my_country();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_retrieve_non_existent_country_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_deleted_my_country();
        admin.Given_I_want_to_retrieve_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_country_ID_with_key("countryId");
    }

    private sealed class AdminActor : ActorWithResponse<GetCountryResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver) : base(apiDriver)
        {
        }

        private Country? MyCountry { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "") =>
            MyCountry = await ApiDriver.Countries.CreateACountryAsync(countryCode: countryCode,
                countryName: countryName,
                cancellationToken: TestContext.Current.CancellationToken);

        public async Task Given_I_have_deleted_my_country()
        {
            Assert.NotNull(MyCountry);

            await ApiDriver.Countries.DeleteACountryAsync(MyCountry.Id, TestContext.Current.CancellationToken);
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

        public void Then_the_problem_details_extensions_should_contain_my_country_ID_with_key(string key)
        {
            Assert.NotNull(MyCountry);

            Then_the_problem_details_extensions_should_contain(key, MyCountry.Id);
        }
    }
}
