using System.Net;
using Eurocentric.Features.AcceptanceTests.AdminApi.V0.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V0.Contests;
using Eurocentric.Features.AdminApi.V0.Contests.Common;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V0.Contests;

public abstract class CreateContestTests : AcceptanceTestBase
{
    protected CreateContestTests(WebAppFixture webAppFixture) : base(webAppFixture) { }

    [Theory]
    [InlineData(2022, "Turin", "Stockholm")]
    [InlineData(2025, "Basel", "Liverpool")]
    public async Task Should_be_able_to_create_a_contest(int contestYear, string cityName, string contestFormat)
    {
        EuroFanActor euroFan = new(AdminApiV0Driver.Create(Client, MajorApiVersion, MinorApiVersion));

        // Given
        euroFan.Given_I_want_to_create_a_contest(contestYear: contestYear, cityName: cityName, contestFormat: contestFormat);

        // When
        await euroFan.When_I_send_my_request();

        // Then
        euroFan.Then_my_request_should_succeed_with_status_code(HttpStatusCode.Created);
        euroFan.Then_the_created_contest_should_match_my_requirements();
    }

    public sealed class V0Point2 : CreateContestTests
    {
        public V0Point2(WebAppFixture webAppFixture) : base(webAppFixture) { }

        private protected override int MajorApiVersion => 0;

        private protected override int MinorApiVersion => 2;
    }

    private sealed class EuroFanActor : ActorBase<CreateContest.Response>
    {
        private readonly AdminApiV0Driver _driver;

        public EuroFanActor(AdminApiV0Driver driver)
        {
            _driver = driver;
        }

        private CreateContest.Request MyRequirements { get; set; } = null!;

        private protected override Func<Task<ResponseOrProblem<CreateContest.Response>>> SendMyRequest { get; set; } = null!;

        public void Given_I_want_to_create_a_contest(string cityName = "CityName",
            int contestYear = 2025,
            string contestFormat = "Stockholm")
        {
            MyRequirements = new CreateContest.Request
            {
                CityName = cityName, ContestYear = contestYear, ContestFormat = Enum.Parse<ContestFormat>(contestFormat)
            };

            SendMyRequest = () => _driver.CreateContestAsync(MyRequirements, TestContext.Current.CancellationToken);
        }

        public void Then_the_created_contest_should_match_my_requirements()
        {
            Assert.NotNull(Response);

            Contest createdContest = Response.Contest;

            Assert.Equal(MyRequirements.ContestYear, createdContest.ContestYear);
            Assert.Equal(MyRequirements.CityName, createdContest.CityName);
            Assert.Equal(MyRequirements.ContestFormat, createdContest.ContestFormat);
        }
    }
}
