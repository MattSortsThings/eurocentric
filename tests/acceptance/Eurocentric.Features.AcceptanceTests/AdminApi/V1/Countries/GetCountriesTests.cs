using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountriesTests : AcceptanceTestBase
{
    public GetCountriesTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_retrieve_all_existing_countries_in_country_code_order()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));

        // Given
        await admin.Given_I_have_created_the_countries("AT", "FI", "BE", "XX", "GB");
        admin.Given_I_want_to_retrieve_all_existing_countries();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_countries_should_be_my_countries_in_country_code_order();
    }

    [Fact]
    public async Task Should_be_able_to_retrieve_an_empty_list_when_no_countries_exist()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));

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
        private readonly AdminApiV1Driver _driver;

        private AdminActor(AdminApiV1Driver driver)
        {
            _driver = driver;
        }

        private List<Country> MyCountries { get; } = [];

        private protected override Func<Task<ResponseOrProblem<GetCountriesResponse>>> SendMyRequest { get; set; } = null!;

        public async Task Given_I_have_created_the_countries(params string[] countryCodes)
        {
            Task<Country>[] tasks = countryCodes.Select(CreateCountryAsync).ToArray();

            Country[] countries = await Task.WhenAll(tasks);

            MyCountries.AddRange(countries);
        }

        public void Given_I_want_to_retrieve_all_existing_countries() =>
            SendMyRequest = () => _driver.GetAllCountriesAsync(TestContext.Current.CancellationToken);

        public void Then_the_retrieved_countries_should_be_my_countries_in_country_code_order()
        {
            Assert.NotNull(Response);

            Assert.Equal(MyCountries.OrderBy(c => c.CountryCode), Response.Countries, EqualityComparers.Country);
        }

        public void Then_the_retrieved_countries_should_be_an_empty_list()
        {
            Assert.NotNull(Response);

            Assert.Empty(Response.Countries);
        }

        private async Task<Country> CreateCountryAsync(string countryCode)
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryCode.ToCountryName() };

            ResponseOrProblem<CreateCountryResponse> responseOrProblem = await _driver.CreateCountryAsync(requestBody);

            return responseOrProblem.AsT0.Data!.Country;
        }

        public static AdminActor WithDriver(AdminApiV1Driver driver) => new(driver);
    }
}
