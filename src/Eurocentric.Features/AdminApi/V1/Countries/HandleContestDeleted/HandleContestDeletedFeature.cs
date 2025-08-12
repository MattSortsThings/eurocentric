using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Features.AdminApi.V1.Countries.HandleContestDeleted;

internal static class HandleContestDeletedFeature
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(AppDbContext dbContext) : IDomainEventHandler<ContestDeletedEvent>
    {
        public async Task OnHandle(ContestDeletedEvent domainEvent, CancellationToken cancellationToken)
        {
            ContestId contestId = domainEvent.Contest.Id;

            CountryId[] countryIds = domainEvent.Contest.Participants
                .Select(participant => participant.ParticipatingCountryId)
                .ToArray();

            Country[] countries = await dbContext.Countries.AsSplitQuery()
                .Where(country => countryIds.Contains(country.Id))
                .ToArrayAsync(cancellationToken);

            foreach (Country country in countries)
            {
                country.RemoveParticipatingContestId(contestId);
            }
        }
    }
}
