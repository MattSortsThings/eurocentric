using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Countries.GetCountry;

internal static class GetCountryFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "countryId")] Guid countryId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(countryId), TypedResults.Ok, cancellationToken);

    internal sealed record Query(Guid CountryId) : IQuery<GetCountryResponse>;

    internal sealed class QueryHandler(AppDbContext dbContext) : IQueryHandler<Query, GetCountryResponse>
    {
        public async Task<ErrorOr<GetCountryResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Country dummyCountry = Country.CreateExample();

            return new GetCountryResponse(dummyCountry with { Id = query.CountryId });
        }
    }
}
