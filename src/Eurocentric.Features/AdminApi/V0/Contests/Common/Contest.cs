namespace Eurocentric.Features.AdminApi.V0.Contests.Common;

public sealed record Contest(Guid Id, int ContestYear, string CityName, ContestFormat ContestFormat);
