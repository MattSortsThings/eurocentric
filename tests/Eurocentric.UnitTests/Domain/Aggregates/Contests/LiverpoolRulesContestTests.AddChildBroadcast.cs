using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.Contests.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests;

public sealed partial class LiverpoolRulesContestTests
{
    [Test]
    [MatrixDataSource]
    public async Task AddChildBroadcast_should_add_specified_child_broadcast(
        [Matrix("26786949-965a-44cc-801a-22c6b5667a3b", "cbafd594-551e-4d87-8089-a8450d4ad059")] string idValue,
        [Matrix(ContestStage.SemiFinal1, ContestStage.SemiFinal2, ContestStage.GrandFinal)] ContestStage contestStage
    )
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse(idValue));

        // Assert
        await Assert.That(sut.ChildBroadcasts).IsEmpty();

        // Act
        sut.AddChildBroadcast(broadcastId, contestStage);

        // Assert
        ChildBroadcast singleChildBroadcast = await Assert.That(sut.ChildBroadcasts).HasSingleItem();

        await Assert
            .That(singleChildBroadcast)
            .HasProperty(broadcast => broadcast.ChildBroadcastId, broadcastId)
            .And.HasProperty(broadcast => broadcast.ContestStage, contestStage)
            .And.HasProperty(broadcast => broadcast.Completed, false);
    }

    [Test]
    public async Task AddChildBroadcast_should_throw_on_child_broadcast_ID_conflict()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("3e6a8110-9250-485f-8e7d-91a64a624a7d"));

        sut.AddChildBroadcast(existingBroadcastId, ContestStage.GrandFinal);

        // Assert
        await Assert
            .That(() => sut.AddChildBroadcast(existingBroadcastId, ContestStage.SemiFinal1))
            .Throws<ArgumentException>()
            .WithMessage("Contest already has a ChildBroadcast with the provided BroadcastId.");

        await Assert.That(sut.ChildBroadcasts).HasSingleItem();
    }

    [Test]
    public async Task AddChildBroadcast_should_throw_on_child_broadcast_contest_stage_conflict()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("3e6a8110-9250-485f-8e7d-91a64a624a7d"));

        sut.AddChildBroadcast(DefaultBroadcastId, ContestStage.GrandFinal);

        // Assert
        await Assert
            .That(() => sut.AddChildBroadcast(broadcastId, ContestStage.GrandFinal))
            .Throws<ArgumentException>()
            .WithMessage("Contest already has a ChildBroadcast with the provided ContestStage.");

        await Assert.That(sut.ChildBroadcasts).HasSingleItem();
    }

    [Test]
    public async Task AddChildBroadcast_should_throw_given_null_broadcastId_arg()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId nullBroadcastId = null!;

        // Assert
        await Assert
            .That(() => sut.AddChildBroadcast(nullBroadcastId, ContestStage.GrandFinal))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'broadcastId')");

        await Assert.That(sut.ChildBroadcasts).IsEmpty();
    }
}
