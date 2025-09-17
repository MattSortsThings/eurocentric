namespace Eurocentric.Apis.Admin.V0.Dtos;

public sealed record Country
{
    public Guid Id { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public ContestRole[] ContestRoles { get; init; } = [];
}
