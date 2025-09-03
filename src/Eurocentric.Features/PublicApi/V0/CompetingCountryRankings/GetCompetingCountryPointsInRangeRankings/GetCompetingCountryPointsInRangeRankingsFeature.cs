using ErrorOr;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Rankings.CompetingCountries;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

internal static class GetCompetingCountryPointsInRangeRankingsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [AsParameters] GetCompetingCountryPointsInRangeRankingsRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(request.ToQuery(), TypedResults.Ok, cancellationToken);

    private static Query ToQuery(this GetCompetingCountryPointsInRangeRankingsRequest request) => new()
    {
        PageIndex = request.PageIndex.GetValueOrDefault(0),
        PageSize = request.PageSize.GetValueOrDefault(10),
        Descending = request.Descending.GetValueOrDefault(false),
        MinPoints = request.MinPoints,
        MaxPoints = request.MaxPoints,
        MinYear = request.MinYear,
        MaxYear = request.MaxYear,
        ContestStage = request.ContestStage,
        VotingMethod = request.VotingMethod,
        VotingCountryCode = request.VotingCountryCode
    };

    private static GetCompetingCountryPointsInRangeRankingsResponse ToResponse(this PointsInRangeRankingsPage page)
    {
        CompetingCountryPointsInRangeRanking[] rankings = page.Rankings.Select(ranking => ranking.ToApiRanking()).ToArray();
        CompetingCountryPointsInRangeMetadata metadata = page.Metadata.ToApiMetadata();

        return new GetCompetingCountryPointsInRangeRankingsResponse(rankings, metadata);
    }

    private static CompetingCountryPointsInRangeRanking ToApiRanking(this PointsInRangeRanking ranking) => new()
    {
        Rank = ranking.Rank,
        CountryCode = ranking.CountryCode,
        CountryName = ranking.CountryName,
        PointsInRange = ranking.PointsInRange,
        PointsAwardsInRange = ranking.PointsAwardsInRange,
        PointsAwards = ranking.PointsAwards,
        Broadcasts = ranking.Broadcasts,
        Contests = ranking.Contests,
        VotingCountries = ranking.VotingCountries
    };

    private static CompetingCountryPointsInRangeMetadata ToApiMetadata(this PointsInRangeMetadata metadata) => new()
    {
        PageIndex = metadata.PageIndex,
        PageSize = metadata.PageSize,
        Descending = metadata.Descending,
        TotalItems = metadata.TotalItems,
        TotalPages = metadata.TotalPages,
        MinPoints = metadata.MinPoints,
        MaxPoints = metadata.MaxPoints,
        MinYear = metadata.MinYear,
        MaxYear = metadata.MaxYear,
        ContestStage = metadata.ContestStage is { } cs ? (QueryableContestStage)(int)cs : null,
        VotingMethod = metadata.VotingMethod is { } vm ? (QueryableVotingMethod)(int)vm : null,
        VotingCountryCode = metadata.VotingCountryCode
    };

    private static PointsInRangeQuery ToRankingsQuery(this Query query) => new()
    {
        PageIndex = query.PageIndex,
        PageSize = query.PageSize,
        Descending = query.Descending,
        MinPoints = query.MinPoints,
        MaxPoints = query.MaxPoints,
        MinYear = query.MinYear,
        MaxYear = query.MaxYear,
        ContestStage =
            query.ContestStage is { } contestStage ? Enum.Parse<ContestStageFilter>(contestStage.ToString()) : null,
        VotingCountryCode = query.VotingCountryCode,
        VotingMethod = query.VotingMethod is { } votingMethod
            ? Enum.Parse<VotingMethodFilter>(votingMethod.ToString())
            : null
    };

    internal sealed record Query : IQuery<GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public bool Descending { get; init; }

        public int MinPoints { get; init; }

        public int MaxPoints { get; init; }

        public int? MinYear { get; init; }

        public int? MaxYear { get; init; }

        public QueryableContestStage? ContestStage { get; init; }

        public QueryableVotingMethod? VotingMethod { get; init; }

        public string? VotingCountryCode { get; init; }
    }

    [UsedImplicitly]
    internal sealed class QueryHandler(ICompetingCountryRankingsGateway rankingsGateway) :
        IQueryHandler<Query, GetCompetingCountryPointsInRangeRankingsResponse>
    {
        public async Task<ErrorOr<GetCompetingCountryPointsInRangeRankingsResponse>> OnHandle(
            Query query,
            CancellationToken cancellationToken) => await query.ToRankingsQuery().ToErrorOr()
            .ThenAsync(rankingsQuery => rankingsGateway.GetPointsInRangeRankingsPageAsync(rankingsQuery, cancellationToken))
            .Then(page => page.ToResponse());
    }
}
