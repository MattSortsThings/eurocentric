using System.Text.Json;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.Utilities;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.AdminApi.V1.Common.Enums;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContestStage = Eurocentric.Domain.Enums.ContestStage;
using DomainBroadcast = Eurocentric.Domain.Broadcasts.Broadcast;
using DomainCompetitor = Eurocentric.Domain.Broadcasts.Competitor;
using DomainJury = Eurocentric.Domain.Broadcasts.Jury;
using DomainTelevote = Eurocentric.Domain.Broadcasts.Televote;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTest(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_able_to_retrieve_broadcast_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-17",
            competingCountryCodes: ["AT", "BE", "DK", "EE"]);
        admin.Given_I_want_to_retrieve_my_broadcast_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code_200_OK();
        admin.Then_the_retrieved_broadcast_should_be_my_broadcast();
    }

    [Theory]
    [InlineData("v1.0")]
    public async Task Should_be_unable_to_retrieve_non_existent_broadcast_by_ID(string apiVersion)
    {
        AdminActor admin = new(AdminApiV1Driver.Create(SutRestClient, apiVersion), SutBackDoor);

        // Given
        await admin.Given_I_have_created_some_countries("AT", "BE", "CZ", "DK", "EE", "FI", "XX");
        await admin.Given_I_have_created_a_Liverpool_format_contest(
            contestYear: 2025,
            group0CountryCode: "XX",
            group1CountryCodes: ["AT", "BE", "CZ"],
            group2CountryCodes: ["DK", "EE", "FI"]);
        await admin.Given_I_have_created_a_child_broadcast_for_my_contest(
            contestStage: "GrandFinal",
            broadcastDate: "2025-05-17",
            competingCountryCodes: ["AT", "BE", "DK", "EE"]);
        await admin.Given_I_have_deleted_my_broadcast();
        admin.Given_I_want_to_retrieve_my_broadcast_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code_404_NotFound();
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Broadcast not found",
            detail: "No broadcast exists with the provided broadcast ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_broadcast_ID_with_key("broadcastId");
    }

    private sealed class AdminActor : ActorWithResponse<GetBroadcastResponse>
    {
        public AdminActor(IAdminApiV1Driver apiDriver, IWebAppFixtureBackDoor backDoor) : base(apiDriver)
        {
            BackDoor = backDoor;
        }

        private IWebAppFixtureBackDoor BackDoor { get; }

        private Dictionary<string, Guid> MyCountryCodesAndIds { get; } = new(7);

        private Contest? MyContest { get; set; }

        private Broadcast? MyBroadcast { get; set; }

        public async Task Given_I_have_created_some_countries(params string[] countryCodes)
        {
            Country[] myCountries = await ApiDriver.Countries.CreateMultipleCountriesAsync(countryCodes,
                TestContext.Current.CancellationToken);

            foreach (Country country in myCountries)
            {
                MyCountryCodesAndIds.Add(country.CountryCode, country.Id);
            }
        }

        public async Task Given_I_have_created_a_Liverpool_format_contest(
            string? group0CountryCode = null,
            string[]? group1CountryCodes = null,
            string[]? group2CountryCodes = null, int contestYear = 0) => MyContest =
            await ApiDriver.Contests.CreateAContestAsync(
                cityName: "CityName",
                contestFormat: ContestFormat.Liverpool,
                contestYear: contestYear,
                group0CountryId: group0CountryCode is null ? null : MyCountryCodesAndIds[group0CountryCode],
                group1CountryIds: group1CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray(),
                group2CountryIds: group2CountryCodes?.Select(code => MyCountryCodesAndIds[code]).ToArray());

        public async Task Given_I_have_created_a_child_broadcast_for_my_contest(string[]? competingCountryCodes,
            string broadcastDate = "",
            string contestStage = "")
        {
            Assert.NotNull(MyContest);

            BroadcastId broadcastId = BroadcastId.Create(DateTimeOffset.UtcNow);
            BroadcastDate date = BroadcastDate.FromValue(DateOnly.ParseExact(broadcastDate, "yyyy-MM-dd")).Value;
            ContestId parentContestId = ContestId.FromValue(MyContest.Id);
            ContestStage broadcastContestStage = Enum.Parse<ContestStage>(contestStage);
            List<DomainCompetitor> competitors = competingCountryCodes?.Select(code => MyCountryCodesAndIds[code])
                .Select(CountryId.FromValue)
                .Select((id, index) => new DomainCompetitor(id, index + 1))
                .ToList() ?? [];
            List<DomainJury> juries = MyCountryCodesAndIds.Values.Select(CountryId.FromValue)
                .Select(id => new DomainJury(id))
                .ToList();
            List<DomainTelevote> televotes = MyCountryCodesAndIds.Values.Select(CountryId.FromValue)
                .Select(id => new DomainTelevote(id))
                .ToList();

            DomainBroadcast broadcast = new(broadcastId,
                date,
                parentContestId,
                broadcastContestStage,
                competitors,
                juries,
                televotes);

            Func<IServiceProvider, Task> add = async sp =>
            {
                await using AppDbContext appDbContext = sp.GetRequiredService<AppDbContext>();
                appDbContext.Broadcasts.Add(broadcast);
                await appDbContext.SaveChangesAsync();
            };

            await BackDoor.ExecuteScopedAsync(add);

            MyBroadcast = broadcast.ToBroadcastDto();
        }

        public void Given_I_want_to_retrieve_my_broadcast_by_its_ID()
        {
            Assert.NotNull(MyBroadcast);

            Guid broadcastId = MyBroadcast.Id;

            SendMyRequest = apiDriver => apiDriver.Broadcasts.GetBroadcast(broadcastId, TestContext.Current.CancellationToken);
        }

        public void Then_the_retrieved_broadcast_should_be_my_broadcast()
        {
            Assert.NotNull(MyBroadcast);
            Assert.NotNull(ResponseObject);

            Assert.Equal(MyBroadcast, ResponseObject.Broadcast, new BroadcastEqualityComparer());
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Assert.NotNull(MyBroadcast);

            BroadcastId broadcastId = BroadcastId.FromValue(MyBroadcast.Id);

            Func<IServiceProvider, Task> delete = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                await dbContext.Broadcasts.Where(broadcast => broadcast.Id == broadcastId)
                    .ExecuteDeleteAsync();
            };

            await BackDoor.ExecuteScopedAsync(delete);
        }

        public void Then_the_problem_details_extensions_should_contain_my_broadcast_ID_with_key(string key)
        {
            Assert.NotNull(MyBroadcast);
            Assert.NotNull(ResponseProblemDetails);

            Assert.Contains(ResponseProblemDetails.Extensions, kvp => kvp.Key == key
                                                                      && kvp.Value is JsonElement e
                                                                      && e.GetGuid() == MyBroadcast.Id);
        }
    }
}
