using ErrorOr;
using Eurocentric.Domain.V0Entities;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableCountries;

internal static class GetQueryableCountriesFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(),
            TypedResults.Ok,
            cancellationToken);

    internal sealed class Query : IQuery<GetQueryableCountriesResponse>;

    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetQueryableCountriesResponse>
    {
        public async Task<ErrorOr<GetQueryableCountriesResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            IQueryable<Participant> queryableParticipants = dbContext.V0Contests.AsNoTracking()
                .AsSplitQuery()
                .Where(contest => contest.Completed)
                .SelectMany(contest => contest.Participants);

            QueryableCountry[] queryableCountries = await dbContext.V0Countries.AsNoTracking()
                .AsSplitQuery()
                .Where(country => queryableParticipants.Any(participant => participant.ParticipatingCountryId == country.Id))
                .OrderBy(country => country.CountryCode)
                .Select(country => new QueryableCountry { CountryCode = country.CountryCode, CountryName = country.CountryName })
                .ToArrayAsync(cancellationToken);

            return new GetQueryableCountriesResponse(queryableCountries);
        }
    }
}
