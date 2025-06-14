using ErrorOr;
using Eurocentric.Domain.Countries;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries;

internal static class DeleteCountry
{
    internal static IEndpointRouteBuilder MapDeleteCountry(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapDelete("countries/{countryId:guid}", HandleAsync)
            .WithName(EndpointIds.Countries.DeleteCountry)
            .WithSummary("Delete a country")
            .WithDescription("Deletes a single country from the system.")
            .HasApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithTags(EndpointTags.Countries);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, NoContent>> HandleAsync([FromRoute(Name = "countryId")] Guid countryId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeCommand(countryId)
        .ThenAsync(command => bus.Send(command, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(_ => TypedResults.NoContent());

    private static ErrorOr<Command> InitializeCommand(Guid countryId) => ErrorOrFactory.From(new Command(countryId));

    internal sealed record Command(Guid CountryId) : ICommand<Deleted>;

    internal sealed class Handler(AppDbContext dbContext) : ICommandHandler<Command, Deleted>
    {
        public async Task<ErrorOr<Deleted>> OnHandle(Command command, CancellationToken cancellationToken) =>
            await GetTrackedCountryToDeleteAsync(CountryId.FromValue(command.CountryId))
                .ThenDo(country => dbContext.Countries.Remove(country))
                .ThenDoAsync(_ => dbContext.SaveChangesAsync(cancellationToken))
                .Then(_ => Result.Deleted);

        private async Task<ErrorOr<Country>> GetTrackedCountryToDeleteAsync(CountryId countryId)
        {
            Country? country = await dbContext.Countries
                .AsSplitQuery()
                .FirstOrDefaultAsync(contest => contest.Id == countryId);

            return country is null
                ? CountryErrors.CountryNotFound(countryId)
                : country.ParticipatingContests.Count > 0
                    ? CountryErrors.CannotDeleteCountry(countryId)
                    : country;
        }
    }
}
