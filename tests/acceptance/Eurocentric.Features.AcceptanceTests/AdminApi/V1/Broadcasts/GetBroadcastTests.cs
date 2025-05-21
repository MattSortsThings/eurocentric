using System.Net;
using System.Text.Json;
using Eurocentric.Domain.Broadcasts;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts.TestUtils;
using Eurocentric.Features.AcceptanceTests.AdminApi.V1.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AdminApi.V1.Broadcasts;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Broadcast = Eurocentric.Features.AdminApi.V1.Common.Dtos.Broadcast;
using Competitor = Eurocentric.Domain.Broadcasts.Competitor;

namespace Eurocentric.Features.AcceptanceTests.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTests : AcceptanceTestBase
{
    public GetBroadcastTests(WebAppFixture webAppFixture) : base(webAppFixture)
    {
    }

    private protected override int ApiMajorVersion => 1;

    private protected override int ApiMinorVersion => 0;

    [Fact]
    public async Task Should_be_able_to_retrieve_a_broadcast_by_its_ID()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_a_broadcast();
        admin.Given_I_want_to_retrieve_my_broadcast_by_its_ID();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_succeed_with_status_code(HttpStatusCode.OK);
        admin.Then_the_retrieved_broadcast_should_be_my_broadcast();
    }

    [Fact]
    public async Task Should_be_unable_to_retrieve_a_non_existent_broadcast_by_its_ID()
    {
        AdminActor admin = AdminActor.WithDriverAndBackdoor(AdminApiV1Driver.Create(Sut, ApiMajorVersion, ApiMinorVersion),
            new WebAppFixtureBackdoor(Sut));

        // Given
        await admin.Given_I_have_created_a_broadcast();
        admin.Given_I_want_to_retrieve_my_broadcast_by_its_ID();
        await admin.Given_I_have_deleted_by_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        admin.Then_my_request_should_fail_with_status_code(HttpStatusCode.NotFound);
        admin.Then_the_problem_details_should_match(status: 404,
            title: "Broadcast not found",
            detail: "No broadcast exists with the provided broadcast ID.");
        admin.Then_the_problem_details_extensions_should_contain_my_broadcast_ID();
    }

    private sealed class AdminActor : ActorWithResponse<GetBroadcastResponse>
    {
        private readonly WebAppFixtureBackdoor _backdoor;
        private readonly AdminApiV1Driver _driver;

        private AdminActor(AdminApiV1Driver driver, WebAppFixtureBackdoor backdoor)
        {
            _driver = driver;
            _backdoor = backdoor;
        }

        private Broadcast? MyBroadcast { get; set; }

        private protected override Func<Task<ResponseOrProblem<GetBroadcastResponse>>> SendMyRequest { get; set; } = null!;

        public static AdminActor WithDriverAndBackdoor(AdminApiV1Driver driver, WebAppFixtureBackdoor backdoor) =>
            new(driver, backdoor);

        public async Task Given_I_have_created_a_broadcast() =>
            MyBroadcast = await _backdoor.CreateAndPersistDummyBroadcastAsync();

        public void Given_I_want_to_retrieve_my_broadcast_by_its_ID() =>
            SendMyRequest = () => _driver.GetBroadcastAsync(MyBroadcast!.Id);

        public async Task Given_I_have_deleted_by_broadcast() => await _backdoor.DeleteBroadcastAsync(MyBroadcast!.Id);

        public void Then_the_retrieved_broadcast_should_be_my_broadcast()
        {
            Assert.NotNull(MyBroadcast);
            Assert.NotNull(Response);

            Assert.Equal(MyBroadcast, Response.Broadcast, new BroadcastEqualityComparer());
        }

        public void Then_the_problem_details_extensions_should_contain_my_broadcast_ID()
        {
            Assert.NotNull(ProblemDetails);

            Assert.Contains(ProblemDetails.Extensions,
                kvp => kvp is { Key: "broadcastId", Value: JsonElement j } && j.GetGuid() == MyBroadcast!.Id);
        }
    }

    private sealed class WebAppFixtureBackdoor(WebAppFixture fixture)
    {
        public async Task<Broadcast> CreateAndPersistDummyBroadcastAsync()
        {
            Func<IServiceProvider, Task<Broadcast>> persist = async sp =>
            {
                Domain.Broadcasts.Broadcast broadcast = CreateDummyBroadcast();
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                dbContext.Broadcasts.Add(broadcast);
                await dbContext.SaveChangesAsync();

                return broadcast.ToBroadcastDto();
            };

            return await fixture.ExecuteScopedAsync(persist);
        }

        public async Task DeleteBroadcastAsync(Guid broadcastId)
        {
            BroadcastId targetId = BroadcastId.FromValue(broadcastId);

            Func<IServiceProvider, Task> delete = async sp =>
            {
                await using AppDbContext dbContext = sp.GetRequiredService<AppDbContext>();
                await dbContext.Broadcasts.Where(broadcast => broadcast.Id == targetId).ExecuteDeleteAsync();
            };

            await fixture.ExecuteScopedAsync(delete);
        }

        private static Domain.Broadcasts.Broadcast CreateDummyBroadcast() => new(
            BroadcastId.FromValue(ExampleValues.BroadcastId),
            ContestId.FromValue(ExampleValues.ContestId),
            ContestStage.GrandFinal,
            [
                new Competitor(CountryId.FromValue(ExampleValues.CountryId1Of3), 1),
                new Competitor(CountryId.FromValue(ExampleValues.CountryId2Of3), 2),
                new Competitor(CountryId.FromValue(ExampleValues.CountryId3Of3), 3)
            ], [
                new Jury(CountryId.FromValue(ExampleValues.CountryId3Of3)),
                new Jury(CountryId.FromValue(ExampleValues.CountryId2Of3)),
                new Jury(CountryId.FromValue(ExampleValues.CountryId1Of3))
            ], [
                new Televote(CountryId.FromValue(ExampleValues.CountryId2Of3)),
                new Televote(CountryId.FromValue(ExampleValues.CountryId1Of3))
            ]);
    }
}
