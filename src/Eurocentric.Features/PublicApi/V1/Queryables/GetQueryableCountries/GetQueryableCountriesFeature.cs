using ErrorOr;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Aggregates.Countries;
using Eurocentric.Features.PublicApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableCountries;

internal static class GetQueryableCountriesFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    private static QueryableCountry ToQueryableCountry(this Country country) => new()
    {
        CountryCode = country.CountryCode.Value, CountryName = country.CountryName.Value
    };

    internal sealed record Query : IQuery<GetQueryableCountriesResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetQueryableCountriesResponse>
    {
        public async Task<ErrorOr<GetQueryableCountriesResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            IQueryable<Participant> queryableParticipants = dbContext.Contests.AsNoTracking()
                .AsSplitQuery()
                .Where(contest => contest.Completed)
                .SelectMany(contest => contest.Participants);

            QueryableCountry[] queryableCountries = await dbContext.Countries.AsNoTracking()
                .AsSplitQuery()
                .Where(country => queryableParticipants.Any(participant => participant.ParticipatingCountryId == country.Id))
                .OrderBy(country => country.CountryCode)
                .Select(country => country.ToQueryableCountry())
                .ToArrayAsync(cancellationToken);

            return new GetQueryableCountriesResponse(queryableCountries);
        }
    }
}
