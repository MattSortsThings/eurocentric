using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Attributes;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Comparers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utils.Extensions.Countries;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries.GetCountry;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_retrieve_requested_country(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");

        await admin.Given_I_want_to_retrieve_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_country_should_be_my_country();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Endpoint_should_fail_on_non_existent_country_requested(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_country(countryCode: "GB", countryName: "United Kingdom");
        await admin.Given_I_have_deleted_my_country();

        await admin.Given_I_want_to_retrieve_my_deleted_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code_404_NotFound();
        await admin.Then_the_response_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        await admin.Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID();
    }

    private sealed class AdminActor(IApiDriver apiDriver) : AdminActorWithResponse<GetCountryResponse>(apiDriver)
    {
        private Country? Country { get; set; }

        private Guid? DeletedCountryId { get; set; }

        public async Task Given_I_have_created_a_country(string countryName = "", string countryCode = "") =>
            Country = await ApiDriver.CreateSingleCountryAsync(countryCode: countryCode, countryName: countryName);

        public async Task Given_I_have_deleted_my_country()
        {
            Country myCountry = await Assert.That(Country).IsNotNull();
            Guid myCountryId = myCountry.Id;

            await ApiDriver.DeleteSingleCountryAsync(myCountryId);

            Country = null;
            DeletedCountryId = myCountryId;
        }

        public async Task Given_I_want_to_retrieve_my_country()
        {
            Country myCountry = await Assert.That(Country).IsNotNull();

            Request = ApiDriver.RequestFactory.Countries.GetCountry(myCountry.Id);
        }

        public async Task Given_I_want_to_retrieve_my_deleted_country()
        {
            Guid myDeletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            Request = ApiDriver.RequestFactory.Countries.GetCountry(myDeletedCountryId);
        }

        public async Task Then_the_retrieved_country_should_be_my_country()
        {
            GetCountryResponse responseBody = await Assert.That(ResponseBody).IsNotNull();
            Country expectedCountry = await Assert.That(Country).IsNotNull();

            await Assert.That(responseBody.Country)
                .IsEqualTo(expectedCountry, new CountryEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_my_deleted_country_ID()
        {
            ProblemDetails problemDetails = await Assert.That(ResponseProblemDetails).IsNotNull();
            Guid myDeletedCountryId = await Assert.That(DeletedCountryId).IsNotNull();

            await Assert.That(problemDetails).HasExtension("countryId", myDeletedCountryId);
        }
    }
}
