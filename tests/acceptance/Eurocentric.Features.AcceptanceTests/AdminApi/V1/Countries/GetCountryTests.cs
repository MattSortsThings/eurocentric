using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests : AcceptanceTestBase
{
    public GetCountryTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    private protected override int MajorApiVersion => 1;

    private protected override int MinorApiVersion => 0;

    [Theory]
    [InlineData("13008a45-7363-4065-bbdb-59643f975903")]
    [InlineData("76ade44c-f947-4043-89ea-a0fe9b189383")]
    public async Task Should_be_able_to_retrieve_a_placeholder_country_by_its_ID(string targetCountryId)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(Client, MajorApiVersion, MinorApiVersion));

        // Given
        admin.Given_I_want_to_retrieve_the_country_with_the_id(targetCountryId);

        // Act
        await admin.When_I_send_my_request();

        // Assert
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_country_should_have_my_target_country_ID();
    }

    private sealed class AdminActor : ActorWithResponse<GetCountry.Response>
    {
        private readonly AdminApiV1Driver _driver;

        public AdminActor(AdminApiV1Driver driver)
        {
            _driver = driver;
        }

        private Guid MyTargetCountryId { get; set; }

        private protected override Func<Task<ResponseOrProblem<GetCountry.Response>>> SendMyRequest { get; set; } = null!;

        public void Given_I_want_to_retrieve_the_country_with_the_id(string countryId)
        {
            MyTargetCountryId = Guid.Parse(countryId);

            SendMyRequest = () => _driver.GetCountryAsync(MyTargetCountryId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_country_should_have_my_target_country_ID()
        {
            Assert.NotNull(Response);

            Assert.Equal(MyTargetCountryId, Response.Country.Id);
        }
    }
}
