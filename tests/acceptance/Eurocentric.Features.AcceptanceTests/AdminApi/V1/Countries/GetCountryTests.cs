using System.Net;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
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

        private Guid MyCountryId { get; set; }

        private Country? MyCountry { get; set; }

        private protected override Func<Task<ResponseOrProblem<GetCountryResponse>>> SendMyRequest { get; set; } = null!;

        public async Task Given_I_have_created_a_country() =>
            MyCountry = await _backdoor.CreateACountryAsync(TestContext.Current.CancellationToken);

        public void Given_I_want_to_retrieve_my_country_by_its_ID()
        {
            MyCountryId = MyCountry!.Id;
            SendMyRequest = () => _driver.GetCountryAsync(MyCountryId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_country_should_be_my_country()
        {
            Assert.NotNull(Response);
            Assert.NotNull(MyCountry);

            Assert.Equal(MyCountry, Response.Country, EqualityComparers.Country);
        }
    }

    private sealed class WebAppBackdoor(WebAppFixture webAppFixture)
    {
        public async Task<Country> CreateACountryAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            Domain.Countries.Country country = Domain.Countries.Country.Create()
                .WithCountryCode(CountryCode.FromValue("GB"))
                .WithName(CountryName.FromValue("United Kingdom"))
                .Build(() => CountryId.Create(DateTimeOffset.UtcNow))
                .Value;

            Action<IServiceProvider> persistCountry = sp =>
            {
                using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Countries.Add(country);
                dbContext.SaveChanges();
            };

            webAppFixture.ExecuteScoped(persistCountry);

            return country.ToCountryDto();
        }
    }
}
