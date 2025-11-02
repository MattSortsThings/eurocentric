using CSharpFunctionalExtensions;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.Events;
using Eurocentric.Domain.ValueObjects;
using JetBrains.Annotations;

namespace Eurocentric.Apis.Admin.V1.Features.Countries;

internal static class HandleContestCreated
{
    [UsedImplicitly]
    internal sealed class DomainEventHandler(ICountryWriteRepository writeRepository)
        : IDomainEventHandler<ContestCreatedEvent>
    {
        public async Task OnHandle(ContestCreatedEvent message, CancellationToken ct)
        {
            (ContestId contestId, IReadOnlyList<Participant> participants, GlobalTelevote? globalTelevote) = (
                message.Contest.Id,
                message.Contest.Participants,
                message.Contest.GlobalTelevote
            );

            if (globalTelevote is not null)
            {
                Country globalTelevoteCountry = await GetTrackedCountryAsync(globalTelevote.VotingCountryId, ct);
                globalTelevoteCountry.AddGlobalTelevoteContestRole(contestId);
                writeRepository.Update(globalTelevoteCountry);
            }

            foreach (Participant participant in participants)
            {
                Country participantCountry = await GetTrackedCountryAsync(participant.ParticipatingCountryId, ct);
                participantCountry.AddParticipantContestRole(contestId);
                writeRepository.Update(participantCountry);
            }

            await Task.CompletedTask;
        }

        private async Task<Country> GetTrackedCountryAsync(CountryId countryId, CancellationToken ct)
        {
            Result<Country, IDomainError> result = await writeRepository.GetTrackedAsync(countryId, ct);

            return result.GetValueOrDefault();
        }
    }
}
