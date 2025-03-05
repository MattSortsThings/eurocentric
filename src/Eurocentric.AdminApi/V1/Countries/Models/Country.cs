namespace Eurocentric.AdminApi.V1.Countries.Models;

public sealed record Country(Guid Id, string CountryCode, string CountryName, CountryType CountryType, Guid[] ContestIds);
