using System.Net;
using System.Text.Json;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Features.AdminApi.V1.Countries.Common;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Countries;

public sealed class GetCountryTests : AcceptanceTestBase
{
    public GetCountryTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    [Fact]
    public async Task Should_be_able_to_retrieve_a_country_by_its_ID()
    {
        AdminActor admin = new(CreateAdminApiV1Driver(), new WebAppBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_a_country();
        admin.Given_I_want_to_retrieve_my_country_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_country_should_be_my_country();
    }

    [Fact]
    public async Task Should_be_unable_to_retrieve_a_non_existent_country_by_its_ID()
    {
        AdminActor admin = new(CreateAdminApiV1Driver(), new WebAppBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_a_country();
        admin.Given_I_want_to_retrieve_my_country_by_its_ID();
        await admin.Given_I_have_deleted_my_country();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.NotFound);
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Country not found",
            detail: "No country exists with the provided country ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_country_ID();
    }

    private protected override AdminApiV1Driver CreateAdminApiV1Driver() => AdminApiV1Driver.Create(Sut, 1, 0);

    private sealed class AdminActor : ActorWithResponse<GetCountryResponse>
    {
        private readonly WebAppBackdoor _backdoor;
        private readonly AdminApiV1Driver _driver;

        public AdminActor(AdminApiV1Driver driver, WebAppBackdoor backdoor)
        {
            _backdoor = backdoor;
            _driver = driver;
        }

        private Country? MyCountry { get; set; }

        private protected override Func<Task<ResponseOrProblem<GetCountryResponse>>> SendMyRequest { get; set; } = null!;

        public async Task Given_I_have_created_a_country() => MyCountry = await CreateCountryAsync("GB");

        public void Given_I_want_to_retrieve_my_country_by_its_ID()
        {
            Guid id = MyCountry!.Id;
            SendMyRequest = () => _driver.GetCountryAsync(id, TestContext.Current.CancellationToken);
        }

        public async Task Given_I_have_deleted_my_country() => await _backdoor.DeleteCountryAsync(MyCountry!.Id);

        public void Then_the_retrieved_country_should_be_my_country()
        {
            Assert.NotNull(Response);
            Assert.NotNull(MyCountry);

            Assert.Equal(MyCountry, Response.Country, EqualityComparers.Country);
        }

        public void Then_the_problem_details_extensions_should_contain_my_country_ID()
        {
            Assert.NotNull(ProblemDetails);

            Assert.Contains(ProblemDetails.Extensions,
                kvp => kvp is { Key: "countryId", Value: JsonElement j } && j.GetGuid() == MyCountry!.Id);
        }

        private async Task<Country> CreateCountryAsync(string countryCode)
        {
            CreateCountryRequest requestBody = new() { CountryCode = countryCode, CountryName = countryCode.ToCountryName() };

            ResponseOrProblem<CreateCountryResponse> responseOrProblem = await _driver.CreateCountryAsync(requestBody);

            return responseOrProblem.AsT0.Data!.Country;
        }
    }

    private sealed class WebAppBackdoor(WebAppFixture webAppFixture)
    {
        public async Task DeleteCountryAsync(Guid countryId, CancellationToken cancellationToken = default)
        {
            Func<IServiceProvider, Task> delete = async sp =>
            {
                await using AsyncServiceScope scope = sp.CreateAsyncScope();
                await using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                CountryId id = CountryId.FromValue(countryId);

                dbContext.Countries.Remove(dbContext.Countries.First(x => x.Id == id));
                await dbContext.SaveChangesAsync(cancellationToken);
            };

            await webAppFixture.ExecuteScopedAsync(delete);
        }
    }
}
