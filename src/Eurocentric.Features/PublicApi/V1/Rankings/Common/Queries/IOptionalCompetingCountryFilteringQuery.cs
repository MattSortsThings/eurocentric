namespace Eurocentric.Features.PublicApi.V1.Rankings.Common.Queries;

internal interface IOptionalCompetingCountryFilteringQuery
{
    string? CompetingCountryCode { get; }
}
