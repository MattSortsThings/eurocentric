using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts.TestUtils;
using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;
using TUnit.Assertions.Enums;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastsTests : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_all_existing_broadcasts_in_broadcast_date_order(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            broadcastDate: "2023-05-01",
            contestStage: "SemiFinal1"
        );
        await admin.Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            broadcastDate: "2016-05-03",
            contestStage: "GrandFinal"
        );
        await admin.Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            broadcastDate: "2025-05-01",
            contestStage: "SemiFinal1"
        );
        await admin.Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            broadcastDate: "2016-05-02",
            contestStage: "SemiFinal2"
        );

        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_empty_list_when_no_broadcasts_exist(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        admin.Given_I_want_to_retrieve_all_existing_broadcasts();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_broadcasts_should_be_an_empty_list();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetBroadcastsResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private List<Broadcast> ExistingBroadcasts { get; } = [];

        public async Task Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            string contestStage = "",
            string broadcastDate = ""
        )
        {
            ContestStage stage = Enum.Parse<ContestStage>(contestStage);
            DateOnly date = DateOnly.Parse(broadcastDate);

            Broadcast broadcast = await Kernel.CreateABroadcastWithDummyContestAndCountriesAsync(
                broadcastDate: date,
                contestStage: stage
            );

            ExistingBroadcasts.Add(broadcast);
        }

        public void Given_I_want_to_retrieve_all_existing_broadcasts() =>
            Request = Kernel.Requests.Broadcasts.GetBroadcasts();

        public async Task Then_the_retrieved_broadcasts_should_be_my_broadcasts_in_broadcast_date_order()
        {
            IOrderedEnumerable<Broadcast> expectedBroadcasts = ExistingBroadcasts.OrderBy(broadcast =>
                broadcast.BroadcastDate
            );

            await Assert
                .That(SuccessResponse?.Data?.Broadcasts)
                .IsEquivalentTo(expectedBroadcasts, new BroadcastEqualityComparer(), CollectionOrdering.Matching);
        }

        public async Task Then_the_retrieved_broadcasts_should_be_an_empty_list() =>
            await Assert.That(SuccessResponse?.Data?.Broadcasts).IsEmpty();
    }
}
