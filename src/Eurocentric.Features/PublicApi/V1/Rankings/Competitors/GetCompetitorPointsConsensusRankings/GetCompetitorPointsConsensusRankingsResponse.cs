using Eurocentric.Features.PublicApi.V1.Rankings.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V1.Rankings.Competitors.GetCompetitorPointsConsensusRankings;

public sealed record GetCompetitorPointsConsensusRankingsResponse(
    CompetitorPointsConsensusRanking[] Rankings,
    CompetitorPointsConsensusFilteringMetadata Filtering,
    PaginationMetadata Pagination);
