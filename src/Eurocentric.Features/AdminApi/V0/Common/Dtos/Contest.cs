using Eurocentric.Features.AdminApi.V0.Common.Enums;

namespace Eurocentric.Features.AdminApi.V0.Common.Dtos;

public sealed record Contest(Guid Id, int ContestYear, string CityName, ContestFormat ContestFormat);
