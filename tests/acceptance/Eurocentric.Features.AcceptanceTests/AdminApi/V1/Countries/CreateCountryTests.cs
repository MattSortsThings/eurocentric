using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Countries;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class CreateCountryTests : AcceptanceTestBase
{
    public CreateCountryTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Theory]
    [InlineData("AT", "Austria")]
    [InlineData("BA", "Bosnia & Herzegovina")]
    [InlineData("XX", "Rest of the World")]
    public async Task Should_be_able_to_create_a_country(string countryCode, string countryName)
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: countryCode, countryName: countryName);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        admin.Then_the_country_should_have_been_correctly_created_from_my_requirements();
        await admin.Then_the_country_should_be_retrievable_by_its_ID();
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_country_with_an_illegal_country_code_value()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));
        const string illegalCountryCode = "11";

        // Given
        admin.Given_I_want_to_create_a_country(countryCode: illegalCountryCode);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal country code value",
            detail: "Country code value must be a string of 2 upper-case letters.");
        admin.Then_the_problem_details_extensions_should_contain("countryCode", illegalCountryCode);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_country_with_an_illegal_country_name_value()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));
        const string illegalCountryName = "";

        // Given
        admin.Given_I_want_to_create_a_country(illegalCountryName);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.UnprocessableEntity);
        admin.Then_the_problem_details_should_match(status: 422,
            title: "Illegal country name value",
            detail: "Country name value must be a non-empty, non-whitespace string of no more than 200 characters.");
        admin.Then_the_problem_details_extensions_should_contain("countryName", illegalCountryName);
    }

    [Fact]
    public async Task Should_be_unable_to_create_a_country_with_a_non_unique_country_code()
    {
        AdminActor admin = AdminActor.WithDriver(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion));
        const string duplicateCountryCode = "GB";

        // Given
        await admin.Given_I_have_created_a_country_with_country_code(duplicateCountryCode);
        admin.Given_I_want_to_create_a_country(countryCode: duplicateCountryCode);

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.Conflict);
        admin.Then_the_problem_details_should_match(status: 409,
            title: "Country code conflict",
            detail: "Country already exists with the provided country code.");
        admin.Then_the_problem_details_extensions_should_contain("countryCode", duplicateCountryCode);
    }

    private sealed class AdminActor : ActorWithResponse<CreateCountryResponse>
    {
        private readonly AdminApiV1Driver _driver;

        private AdminActor(AdminApiV1Driver driver)
        {
            _driver = driver;
        }

        private CreateCountryRequest? MyRequirements { get; set; }

        public async Task Given_I_have_created_a_country_with_country_code(string countryCode)
        {
            CreateCountryRequest request = new() { CountryCode = countryCode, CountryName = "CountryName" };

            _ = await _driver.CreateCountryAsync(request, TestContext.Current.CancellationToken);
        }

        public void Given_I_want_to_create_a_country(string countryName = "CountryName", string countryCode = "AA")
        {
            MyRequirements = new CreateCountryRequest { CountryCode = countryCode, CountryName = countryName };

            SendMyRequest = () => _driver.CreateCountryAsync(MyRequirements, TestContext.Current.CancellationToken);
        }

        public void Then_the_country_should_have_been_correctly_created_from_my_requirements()
        {
            Assert.NotNull(Response);
            Assert.NotNull(MyRequirements);

            Country createdCountry = Response.Country;

            Assert.Equal(MyRequirements.CountryCode, createdCountry.CountryCode);
            Assert.Equal(MyRequirements.CountryName, createdCountry.Name);
            Assert.Empty(createdCountry.ContestMemos);
        }

        public async Task Then_the_country_should_be_retrievable_by_its_ID()
        {
            Assert.NotNull(Response);

            Country createdCountry = Response.Country;
            Country retrievedCountry = await GetCountryAsync(createdCountry.Id);

            Assert.Equal(createdCountry, retrievedCountry, new CountryEqualityComparer());
        }

        private async Task<Country> GetCountryAsync(Guid countryId, CancellationToken cancellationToken = default)
        {
            ResponseOrProblem<GetCountryResponse> responseOrProblem =
                await _driver.GetCountryAsync(countryId, cancellationToken);

            return responseOrProblem.AsT0.Data!.Country;
        }

        public static AdminActor WithDriver(AdminApiV1Driver driver) => new(driver);
    }
}
