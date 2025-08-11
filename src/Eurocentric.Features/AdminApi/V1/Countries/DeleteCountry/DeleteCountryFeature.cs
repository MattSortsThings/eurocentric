using ErrorOr;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries.DeleteCountry;

internal static class DeleteCountryFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "countryId")] Guid countryId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Command(countryId),
            _ => TypedResults.NoContent(),
            cancellationToken);

    private static async Task<ErrorOr<Country>> FailIfContestParticipatesInAnyContestsAsync(this Task<ErrorOr<Country>> task)
    {
        ErrorOr<Country> result = await task;

        return result.FailIf(contest => contest.ParticipatingContestIds.Count != 0,
                CountryErrors.CountryDeletionBlocked())
            .Then(contest => contest);
    }

    internal sealed record Command(Guid CountryId) : ICommand<Deleted>;

    [UsedImplicitly]
    internal sealed class CommandHandler(AppDbContext dbContext) : ICommandHandler<Command, Deleted>
    {
        public async Task<ErrorOr<Deleted>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedCountryAsync(command.CountryId)
                .FailIfContestParticipatesInAnyContestsAsync()
                .ThenDo(country => dbContext.Countries.Remove(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(_ => Result.Deleted);

        private async Task<ErrorOr<Country>> GetTrackedCountryAsync(Guid contestId)
        {
            CountryId id = CountryId.FromValue(contestId);

            Country? country = await dbContext.Countries.FirstOrDefaultAsync(country => country.Id == id);

            return country is null
                ? CountryErrors.CountryNotFound(id)
                : country;
        }
    }
}
