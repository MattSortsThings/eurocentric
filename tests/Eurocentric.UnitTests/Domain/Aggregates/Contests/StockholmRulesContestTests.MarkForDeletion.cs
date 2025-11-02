using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Events;
using Eurocentric.UnitTests.Domain.Aggregates.TestUtils;

namespace Eurocentric.UnitTests.Domain.Aggregates.Contests;

public sealed partial class StockholmRulesContestTests
{
    [Test]
    public async Task MarkForDeletion_should_add_ContestDeletedEvent()
    {
        // Arrange
        StockholmRulesContest sut = CreateAStockholmRulesContest(
            semiFinal1CountryIds: [CountryIds.At, CountryIds.Be, CountryIds.Cz],
            semiFinal2CountryIds: [CountryIds.Dk, CountryIds.Ee, CountryIds.Fi]
        );

        // Assert
        await Assert.That(sut.DetachAllDomainEvents()).IsEmpty();

        // Act
        sut.MarkForDeletion();

        // Assert
        IDomainEvent singleEvent = await Assert.That(sut.DetachAllDomainEvents()).HasSingleItem();

        await Assert
            .That(singleEvent)
            .IsTypeOf<ContestDeletedEvent>()
            .And.Member(contestDeletedEvent => contestDeletedEvent.Contest, source => source.IsSameReferenceAs(sut));
    }
}
