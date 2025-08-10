using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Aggregates.Contests.Utils;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests;

public sealed partial class LiverpoolFormatContestTests
{
    [Test]
    public async Task CompleteChildBroadcast_should_set_ChildBroadcast_Completed_to_true()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("2eab2df8-49d0-4f47-b8c1-7602675a3809"));

        sut.AddChildBroadcast(broadcastId, ContestStage.SemiFinal1);

        // Act
        sut.CompleteChildBroadcast(broadcastId);

        // Assert
        await Assert.That(sut.ChildBroadcasts)
            .HasCount(1)
            .And.Contains(Matchers.ChildBroadcastCompleted(broadcastId, ContestStage.SemiFinal1));
    }

    [Test]
    public async Task CompleteChildBroadcast_should_set_Completed_to_false_when_fewer_than_3_ChildBroadcasts()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId broadcastId1Of2 = BroadcastId.FromValue(Guid.Parse("2eab2df8-49d0-4f47-b8c1-7602675a3809"));
        BroadcastId broadcastId2Of2 = BroadcastId.FromValue(Guid.Parse("391772c5-e360-4c8a-86a2-fdfe6c476f20"));

        sut.AddChildBroadcast(broadcastId1Of2, ContestStage.SemiFinal1);
        sut.AddChildBroadcast(broadcastId2Of2, ContestStage.SemiFinal2);

        // Act
        sut.CompleteChildBroadcast(broadcastId1Of2);

        // Assert
        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task CompleteChildBroadcast_should_set_Completed_to_false_when_3_ChildBroadcasts_still_not_all_Completed()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId broadcastId1Of3 = BroadcastId.FromValue(Guid.Parse("2eab2df8-49d0-4f47-b8c1-7602675a3809"));
        BroadcastId broadcastId2Of3 = BroadcastId.FromValue(Guid.Parse("391772c5-e360-4c8a-86a2-fdfe6c476f20"));
        BroadcastId broadcastId3Of3 = BroadcastId.FromValue(Guid.Parse("f3300bfa-8930-4b12-b203-cb46bedb828d"));

        sut.AddChildBroadcast(broadcastId1Of3, ContestStage.SemiFinal1);
        sut.AddChildBroadcast(broadcastId2Of3, ContestStage.SemiFinal2);
        sut.AddChildBroadcast(broadcastId3Of3, ContestStage.GrandFinal);

        // Act
        sut.CompleteChildBroadcast(broadcastId1Of3);

        // Assert
        await Assert.That(sut.Completed).IsFalse();
    }

    [Test]
    public async Task CompleteChildBroadcast_should_set_Completed_to_true_when_3_ChildBroadcasts_now_all_Completed()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId broadcastId1Of3 = BroadcastId.FromValue(Guid.Parse("2eab2df8-49d0-4f47-b8c1-7602675a3809"));
        BroadcastId broadcastId2Of3 = BroadcastId.FromValue(Guid.Parse("391772c5-e360-4c8a-86a2-fdfe6c476f20"));
        BroadcastId broadcastId3Of3 = BroadcastId.FromValue(Guid.Parse("f3300bfa-8930-4b12-b203-cb46bedb828d"));

        sut.AddChildBroadcast(broadcastId1Of3, ContestStage.SemiFinal1);

        sut.AddChildBroadcast(broadcastId2Of3, ContestStage.SemiFinal2);
        sut.CompleteChildBroadcast(broadcastId2Of3);

        sut.AddChildBroadcast(broadcastId3Of3, ContestStage.GrandFinal);
        sut.CompleteChildBroadcast(broadcastId3Of3);

        // Act
        sut.CompleteChildBroadcast(broadcastId1Of3);

        // Assert
        await Assert.That(sut.Completed).IsTrue();
    }

    [Test]
    public async Task CompleteChildBroadcast_should_throw_given_null_broadcastId_arg()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        Action act = () => sut.CompleteChildBroadcast(null!);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'broadcastId')")
            .WithParameterName("broadcastId");
    }

    [Test]
    public async Task CompleteChildBroadcast_should_throw_given_broadcastId_arg_matching_no_ChildBroadcast()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("2eab2df8-49d0-4f47-b8c1-7602675a3809"));

        Action act = () => sut.CompleteChildBroadcast(broadcastId);

        // Assert
        await Assert.That(act).Throws<ArgumentException>()
            .WithMessage("Contest contains no ChildBroadcast object with the provided BroadcastId value.");
    }
}
