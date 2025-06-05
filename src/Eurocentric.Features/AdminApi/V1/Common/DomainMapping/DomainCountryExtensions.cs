using Eurocentric.Features.AdminApi.V1.Common.Enums;
using ContestMemoDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.ContestMemo;
using CountryDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Country;
using DomainContestMemo = Eurocentric.Domain.ValueObjects.ContestMemo;
using DomainCountry = Eurocentric.Domain.Countries.Country;

namespace Eurocentric.Features.AdminApi.V1.Common.DomainMapping;

internal static class DomainCountryExtensions
{
    internal static CountryDto ToCountryDto(this DomainCountry country) => new()
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        CountryName = country.CountryName.Value,
        ParticipatingContests = country.ParticipatingContests.Select(memo => memo.ToContestMemoDto()).ToArray()
    };

    private static ContestMemoDto ToContestMemoDto(this DomainContestMemo memo) => new(memo.ContestId.Value,
        Enum.Parse<ContestStatus>(memo.ContestStatus.ToString()));
}
