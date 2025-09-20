using Eurocentric.Apis.Admin.V0.Features.Countries;
using Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils;
using Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Utils.Attributes;
using Eurocentric.WebApp.AcceptanceTests.Utils;

namespace Eurocentric.WebApp.AcceptanceTests.AdminApi.V0.Countries;

public sealed class GetCountriesTests : CleanSerialAcceptanceTest
{
    [Test]
    [V0Point2AndUp]
    public async Task Should_retrieve_empty_list_when_no_countries_exist(string apiVersion)
    {
        AdminActor admin = new(ApiDriver.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code_200_OK();
        await admin.Then_the_retrieved_countries_should_be_an_empty_list();
    }

    private sealed class AdminActor : AdminActorWithResponse<GetCountries.Response>
    {
        public AdminActor(IApiDriver apiDriver) : base(apiDriver) { }

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            Request = ApiDriver.RequestFactory.Countries.GetCountries();

        public async Task Then_the_retrieved_countries_should_be_an_empty_list()
        {
            GetCountries.Response responseObject = await Assert.That(ResponseObject).IsNotNull();

            await Assert.That(responseObject.Countries).IsEmpty();
        }
    }
}
