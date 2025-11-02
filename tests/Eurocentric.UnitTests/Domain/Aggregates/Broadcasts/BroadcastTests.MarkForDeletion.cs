using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Events;
using Eurocentric.UnitTests.Domain.Aggregates.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Broadcasts;

public sealed partial class BroadcastTests
{
    [Test]
    public async Task MarkForDeletion_should_add_BroadcastDeletedEvent()
    {
        // Arrange
        Broadcast sut = CreateTelevoteOnlyBroadcast(
            competingCountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            extraVotingCountryIds: [CountryIds.Xx]
        );

        // Act
        sut.MarkForDeletion();

        // Assert
        IDomainEvent singleEvent = await Assert.That(sut.DetachAllDomainEvents()).HasSingleItem();

        await Assert
            .That(singleEvent)
            .IsTypeOf<BroadcastDeletedEvent>()
            .And.Member(
                broadcastDeletedEvent => broadcastDeletedEvent.Broadcast,
                source => source.IsSameReferenceAs(sut)
            );
    }
}
