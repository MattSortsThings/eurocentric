using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries.HandleContestCreated;

internal static class HandleContestCreatedFeature
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(AppDbContext dbContext) : IConsumer<ContestCreatedEvent>
    {
        public async Task OnHandle(ContestCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            ContestId contestId = domainEvent.Contest.Id;

            CountryId[] countryIds = domainEvent.Contest.Participants
                .Select(participant => participant.ParticipatingCountryId)
                .ToArray();

            Country[] countries = await dbContext.Countries.AsSingleQuery()
                .Where(country => countryIds.Contains(country.Id))
                .ToArrayAsync(cancellationToken);

            foreach (Country country in countries)
            {
                country.AddParticipatingContestId(contestId);
            }
        }
    }
}
