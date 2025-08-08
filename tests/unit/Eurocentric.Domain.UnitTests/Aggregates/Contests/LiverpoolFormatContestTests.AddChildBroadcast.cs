using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Identifiers;
using TUnit.Assertions.AssertConditions.Throws;

namespace Eurocentric.Domain.UnitTests.Aggregates.Contests;

public sealed partial class LiverpoolFormatContestTests
{
    [Test]
    public async Task AddChildBroadcast_should_add_ChildBroadcast_with_provided_BroadcastId_and_ContestStage()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId broadcastId = BroadcastId.FromValue(Guid.Parse("eb3036eb-a2b3-450f-b57b-2bfd4516b523"));
        const ContestStage contestStage = ContestStage.GrandFinal;

        // Assert
        await Assert.That(sut.ChildBroadcasts).IsEmpty();

        // Act
        sut.AddChildBroadcast(broadcastId, contestStage);

        // Assert
        ChildBroadcast? singleChildBroadcast = await Assert.That(sut.ChildBroadcasts).HasSingleItem();

        await Assert.That(singleChildBroadcast).IsNotNull()
            .And.HasMember(broadcast => broadcast.BroadcastId).EqualTo(broadcastId)
            .And.HasMember(broadcast => broadcast.ContestStage).EqualTo(contestStage)
            .And.HasMember(broadcast => broadcast.Completed).EqualTo(false);
    }

    [Test]
    public async Task AddChildBroadcast_should_throw_given_null_broadcastId_arg()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        const ContestStage arbitraryContestStage = default;

        Action act = () => sut.AddChildBroadcast(null!, arbitraryContestStage);

        // Assert
        await Assert.That(act).Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'broadcastId')")
            .WithParameterName("broadcastId");
    }

    [Test]
    public async Task AddChildBroadcast_should_throw_given_broadcastId_arg_matching_existing_ChildBroadcast()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        BroadcastId sharedBroadcastId = BroadcastId.FromValue(Guid.Parse("eb3036eb-a2b3-450f-b57b-2bfd4516b523"));

        const ContestStage existingContestStage = ContestStage.GrandFinal;
        const ContestStage newContestStage = ContestStage.SemiFinal1;

        sut.AddChildBroadcast(sharedBroadcastId, existingContestStage);

        Action act = () => sut.AddChildBroadcast(sharedBroadcastId, newContestStage);

        // Assert
        await Assert.That(act).Throws<ArgumentException>()
            .WithMessage("Contest already contains a ChildBroadcast object with the same BroadcastId value.");
    }

    [Test]
    public async Task AddChildBroadcast_should_throw_given_contestStage_arg_matching_existing_ChildBroadcast()
    {
        // Arrange
        LiverpoolFormatContest sut = CreateContest(group0CountryId: XxId,
            group1CountryIds: [AtId, BeId, CzId],
            group2CountryIds: [DkId, EeId, FiId]);

        const ContestStage sharedContestStage = ContestStage.SemiFinal1;

        BroadcastId existingBroadcastId = BroadcastId.FromValue(Guid.Parse("eb3036eb-a2b3-450f-b57b-2bfd4516b523"));
        BroadcastId newBroadcastId = BroadcastId.FromValue(Guid.Parse("093de9cf-0010-460c-840f-59bdfdf0cf81"));

        sut.AddChildBroadcast(existingBroadcastId, sharedContestStage);

        Action act = () => sut.AddChildBroadcast(newBroadcastId, sharedContestStage);

        // Assert
        await Assert.That(act).Throws<ArgumentException>()
            .WithMessage("Contest already contains a ChildBroadcast object with the same ContestStage value.");
    }
}
