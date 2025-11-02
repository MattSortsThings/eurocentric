using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests;

public sealed partial class LiverpoolRulesContestTests
{
    [Test]
    public async Task CompleteChildBroadcast_should_set_Queryable_false_and_remove_ChildBroadcast_matching_broadcastId()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId semiFinal1BroadcastId = BroadcastId.FromValue(Guid.Parse("f56ba494-42f6-4d26-8403-5dffbd27fe5a"));
        BroadcastId semiFinal2BroadcastId = BroadcastId.FromValue(Guid.Parse("b39486d0-0408-4e1f-9203-e83fefe6e5e1"));
        BroadcastId grandFinalBroadcastId = BroadcastId.FromValue(Guid.Parse("02d1a3f6-1291-40a6-a60d-746ee9304881"));

        sut.AddChildBroadcast(semiFinal1BroadcastId, ContestStage.SemiFinal1);
        sut.AddChildBroadcast(semiFinal2BroadcastId, ContestStage.SemiFinal2);
        sut.AddChildBroadcast(grandFinalBroadcastId, ContestStage.GrandFinal);

        sut.CompleteChildBroadcast(semiFinal1BroadcastId);
        sut.CompleteChildBroadcast(semiFinal2BroadcastId);
        sut.CompleteChildBroadcast(grandFinalBroadcastId);

        // Assert
        await Assert.That(sut.Queryable).IsTrue();

        // Act
        sut.RemoveChildBroadcast(semiFinal1BroadcastId);

        // Assert
        await Assert.That(sut.Queryable).IsFalse();

        await Assert
            .That(sut.ChildBroadcasts)
            .HasCount(2)
            .And.DoesNotContain(broadcast => broadcast.ContestStage == ContestStage.SemiFinal1);
    }

    [Test]
    public async Task RemoveChildBroadcast_should_throw_given_null_broadcastId_arg()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId nullBroadcastId = null!;

        // Assert
        await Assert
            .That(() => sut.RemoveChildBroadcast(nullBroadcastId))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'broadcastId')");
    }

    [Test]
    public async Task RemoveChildBroadcast_should_throw_given_broadcastId_arg_matching_no_ChildBroadcast()
    {
        // Arrange
        LiverpoolRulesContest sut = CreateALiverpoolRulesContest(
            contestYear: ContestYear2016,
            globalTelevoteCountryId: CountryIds.Xx,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId orphanBroadcastId = BroadcastId.FromValue(Guid.Parse("f56ba494-42f6-4d26-8403-5dffbd27fe5a"));

        // Assert
        await Assert
            .That(() => sut.RemoveChildBroadcast(orphanBroadcastId))
            .Throws<ArgumentException>()
            .WithMessage("Contest has no ChildBroadcast with the provided BroadcastId.");
    }
}
