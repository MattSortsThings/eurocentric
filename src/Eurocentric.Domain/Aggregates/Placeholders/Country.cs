using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Country
{
    public Guid Id { get; init; }

    public string CountryCode { get; init; } = string.Empty;

    public string CountryName { get; init; } = string.Empty;

    public CountryType CountryType { get; init; }

    public List<ContestRole> ContestRoles { get; init; } = [];
}
