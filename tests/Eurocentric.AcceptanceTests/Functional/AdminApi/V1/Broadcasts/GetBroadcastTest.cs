using Eurocentric.AcceptanceTests.Functional.AdminApi.V1.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.Apis.Admin.V1.Dtos.Broadcasts;
using Eurocentric.Apis.Admin.V1.Enums;
using Eurocentric.Apis.Admin.V1.Features.Broadcasts;

namespace Eurocentric.AcceptanceTests.Functional.AdminApi.V1.Broadcasts;

public sealed class GetBroadcastTest : SerialCleanAcceptanceTest
{
    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_retrieve_requested_broadcast(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            broadcastDate: "2025-05-17",
            contestStage: "GrandFinal"
        );

        await admin.Given_I_want_to_retrieve_my_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_SUCCEED_with_status_code(200);
        await admin.Then_the_retrieved_broadcast_should_be_my_broadcast();
    }

    [Test]
    [ApiVersion1Point0AndUp]
    public async Task Should_fail_on_broadcast_not_found(string apiVersion)
    {
        Admin admin = new(AdminKernel.Create(SystemUnderTest, apiVersion));

        // Given
        await admin.Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            broadcastDate: "2025-05-17",
            contestStage: "GrandFinal"
        );
        await admin.Given_I_have_deleted_my_broadcast();

        await admin.Given_I_want_to_retrieve_the_deleted_broadcast();

        // When
        await admin.When_I_send_my_request();

        // Then
        await admin.Then_my_request_should_FAIL_with_status_code(404);
        await admin.Then_the_response_problem_details_should_match(
            status: 404,
            title: "Broadcast not found",
            detail: "The requested broadcast does not exist."
        );
        await admin.Then_the_response_problem_details_extensions_should_include_the_deleted_broadcast_ID();
    }

    private sealed class Admin(AdminKernel kernel) : AdminActor<GetBroadcastResponse>
    {
        private protected override AdminKernel Kernel { get; } = kernel;

        private Broadcast? ExistingBroadcast { get; set; }

        private Guid? DeletedBroadcastId { get; set; }

        public async Task Given_I_have_created_a_broadcast_with_dummy_contest_and_countries(
            string contestStage = "",
            string broadcastDate = ""
        )
        {
            ContestStage stage = Enum.Parse<ContestStage>(contestStage);
            DateOnly date = DateOnly.Parse(broadcastDate);

            ExistingBroadcast = await Kernel.CreateABroadcastWithDummyContestAndCountriesAsync(
                broadcastDate: date,
                contestStage: stage
            );
        }

        public async Task Given_I_have_deleted_my_broadcast()
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            await Kernel.DeleteABroadcastAsync(broadcastId);

            ExistingBroadcast = null;
            DeletedBroadcastId = broadcastId;
        }

        public async Task Given_I_want_to_retrieve_my_broadcast()
        {
            Guid broadcastId = await Assert.That(ExistingBroadcast?.Id).IsNotNull();

            Request = Kernel.Requests.Broadcasts.GetBroadcast(broadcastId);
        }

        public async Task Given_I_want_to_retrieve_the_deleted_broadcast()
        {
            Guid broadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            Request = Kernel.Requests.Broadcasts.GetBroadcast(broadcastId);
        }

        public async Task Then_the_retrieved_broadcast_should_be_my_broadcast()
        {
            Broadcast expectedBroadcast = await Assert.That(ExistingBroadcast).IsNotNull();

            await Assert
                .That(SuccessResponse?.Data?.Broadcast)
                .IsEqualTo(expectedBroadcast, new BroadcastEqualityComparer());
        }

        public async Task Then_the_response_problem_details_extensions_should_include_the_deleted_broadcast_ID()
        {
            Guid deletedBroadcastId = await Assert.That(DeletedBroadcastId).IsNotNull();

            await Assert.That(FailureResponse?.Data).IsNotNull().And.HasExtension("broadcastId", deletedBroadcastId);
        }
    }
}
