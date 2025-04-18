namespace Eurocentric.Features.AdminApi.V0.Contests.Models;

public record Contest(Guid Id, int ContestYear, string CityName, ContestFormat Format, bool Completed);
