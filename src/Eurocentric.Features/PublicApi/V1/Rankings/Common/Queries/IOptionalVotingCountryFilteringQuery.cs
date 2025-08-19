namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IOptionalVotingCountryFilteringQuery
{
    string? VotingCountryCode { get; }
}
