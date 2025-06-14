using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Eurocentric.Features.AdminApi.V1.Countries;

internal static class HandleContestStatusUpdated
{
    internal sealed class Handler(AppDbContext dbContext) : IDomainEventHandler<ContestStatusUpdatedEvent>
    {
        public async Task OnHandle(ContestStatusUpdatedEvent message, CancellationToken cancellationToken)
        {
            ContestId contestId = message.Contest.Id;
            ContestStatus contestStatus = message.Contest.ContestStatus;

            IEnumerable<CountryId> participatingCountryIds = message.Contest.Participants.Select(participant =>
                participant.ParticipatingCountryId);

            Country[] countriesToUpdate = await dbContext.Countries
                .AsSplitQuery()
                .Join(participatingCountryIds,
                    country => country.Id,
                    id => id,
                    (country, _) => country)
                .ToArrayAsync(cancellationToken);

            foreach (Country country in countriesToUpdate)
            {
                country.ReplaceMemo(contestId, contestStatus);
                dbContext.Countries.Update(country);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
