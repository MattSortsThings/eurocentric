using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.Domain.Aggregates.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests;

public sealed partial class StockholmRulesContestTests
{
    [Test]
    public async Task CompleteChildBroadcast_should_set_ChildBroadcast_Completed_to_true()
    {
        // Arrange
        StockholmRulesContest sut = CreateAStockholmRulesContest(
            contestYear: ContestYear2016,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId semiFinal1BroadcastId = BroadcastId.FromValue(Guid.Parse("f56ba494-42f6-4d26-8403-5dffbd27fe5a"));
        BroadcastId semiFinal2BroadcastId = BroadcastId.FromValue(Guid.Parse("b39486d0-0408-4e1f-9203-e83fefe6e5e1"));

        sut.AddChildBroadcast(semiFinal1BroadcastId, ContestStage.SemiFinal1);
        sut.AddChildBroadcast(semiFinal2BroadcastId, ContestStage.SemiFinal2);

        // Assert
        await Assert.That(sut.ChildBroadcasts).ContainsOnly(broadcast => !broadcast.Completed);

        // Act
        sut.CompleteChildBroadcast(semiFinal1BroadcastId);

        // Assert
        await Assert
            .That(sut.ChildBroadcasts)
            .Contains(broadcast => broadcast.ChildBroadcastId.Equals(semiFinal1BroadcastId) && broadcast.Completed)
            .And.Contains(broadcast =>
                broadcast.ChildBroadcastId.Equals(semiFinal2BroadcastId) && !broadcast.Completed
            );
    }

    [Test]
    public async Task CompleteChildBroadcast_should_set_Queryable_false_when_fewer_than_3_ChildBroadcasts()
    {
        // Arrange
        StockholmRulesContest sut = CreateAStockholmRulesContest(
            contestYear: ContestYear2016,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId semiFinal1BroadcastId = BroadcastId.FromValue(Guid.Parse("f56ba494-42f6-4d26-8403-5dffbd27fe5a"));
        BroadcastId semiFinal2BroadcastId = BroadcastId.FromValue(Guid.Parse("b39486d0-0408-4e1f-9203-e83fefe6e5e1"));

        sut.AddChildBroadcast(semiFinal1BroadcastId, ContestStage.SemiFinal1);
        sut.AddChildBroadcast(semiFinal2BroadcastId, ContestStage.SemiFinal2);

        sut.CompleteChildBroadcast(semiFinal1BroadcastId);

        // Assert
        await Assert.That(sut.Queryable).IsFalse();

        // Act
        sut.CompleteChildBroadcast(semiFinal2BroadcastId);

        // Assert
        await Assert.That(sut.Queryable).IsFalse();
    }

    [Test]
    public async Task CompleteChildBroadcast_should_set_Queryable_false_when_3_ChildBroadcasts_not_all_PointsAwarded()
    {
        // Arrange
        StockholmRulesContest sut = CreateAStockholmRulesContest(
            contestYear: ContestYear2016,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId semiFinal1BroadcastId = BroadcastId.FromValue(Guid.Parse("f56ba494-42f6-4d26-8403-5dffbd27fe5a"));
        BroadcastId semiFinal2BroadcastId = BroadcastId.FromValue(Guid.Parse("b39486d0-0408-4e1f-9203-e83fefe6e5e1"));
        BroadcastId grandFinalBroadcast = BroadcastId.FromValue(Guid.Parse("02d1a3f6-1291-40a6-a60d-746ee9304881"));

        sut.AddChildBroadcast(semiFinal1BroadcastId, ContestStage.SemiFinal1);
        sut.AddChildBroadcast(semiFinal2BroadcastId, ContestStage.SemiFinal2);
        sut.AddChildBroadcast(grandFinalBroadcast, ContestStage.GrandFinal);

        sut.CompleteChildBroadcast(semiFinal1BroadcastId);

        // Assert
        await Assert.That(sut.Queryable).IsFalse();

        // Act
        sut.CompleteChildBroadcast(semiFinal2BroadcastId);

        // Assert
        await Assert.That(sut.Queryable).IsFalse();
    }

    [Test]
    public async Task CompleteChildBroadcast_should_set_Queryable_false_when_3_ChildBroadcasts_all_PointsAwarded()
    {
        // Arrange
        StockholmRulesContest sut = CreateAStockholmRulesContest(
            contestYear: ContestYear2016,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId semiFinal1BroadcastId = BroadcastId.FromValue(Guid.Parse("f56ba494-42f6-4d26-8403-5dffbd27fe5a"));
        BroadcastId semiFinal2BroadcastId = BroadcastId.FromValue(Guid.Parse("b39486d0-0408-4e1f-9203-e83fefe6e5e1"));
        BroadcastId grandFinalBroadcast = BroadcastId.FromValue(Guid.Parse("02d1a3f6-1291-40a6-a60d-746ee9304881"));

        sut.AddChildBroadcast(semiFinal1BroadcastId, ContestStage.SemiFinal1);
        sut.AddChildBroadcast(semiFinal2BroadcastId, ContestStage.SemiFinal2);
        sut.AddChildBroadcast(grandFinalBroadcast, ContestStage.GrandFinal);

        sut.CompleteChildBroadcast(semiFinal1BroadcastId);
        sut.CompleteChildBroadcast(semiFinal2BroadcastId);

        // Assert
        await Assert.That(sut.Queryable).IsFalse();

        // Act
        sut.CompleteChildBroadcast(grandFinalBroadcast);

        // Assert
        await Assert.That(sut.Queryable).IsTrue();
    }

    [Test]
    public async Task CompleteChildBroadcast_should_throw_given_null_broadcastId_arg()
    {
        // Arrange
        StockholmRulesContest sut = CreateAStockholmRulesContest(
            contestYear: ContestYear2016,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId nullBroadcastId = null!;

        // Assert
        await Assert
            .That(() => sut.CompleteChildBroadcast(nullBroadcastId))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'broadcastId')");
    }

    [Test]
    public async Task CompleteChildBroadcast_should_throw_given_broadcastId_arg_matching_no_ChildBroadcast()
    {
        // Arrange
        StockholmRulesContest sut = CreateAStockholmRulesContest(
            contestYear: ContestYear2016,
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        BroadcastId orphanBroadcastId = BroadcastId.FromValue(Guid.Parse("f56ba494-42f6-4d26-8403-5dffbd27fe5a"));

        // Assert
        await Assert
            .That(() => sut.CompleteChildBroadcast(orphanBroadcastId))
            .Throws<ArgumentException>()
            .WithMessage("Contest has no ChildBroadcast with the provided BroadcastId.");
    }
}
