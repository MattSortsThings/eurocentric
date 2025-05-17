using Eurocentric.Features.AdminApi.V1.Common.Enums;

namespace Eurocentric.Features.AdminApi.V1.Common.Dtos;

internal static class Mapping
{
    internal static Country ToCountryDto(this Domain.Countries.Country country) => new()
    {
        Id = country.Id.Value,
        CountryCode = country.CountryCode.Value,
        Name = country.Name.Value,
        ContestMemos = country.ContestMemos.Select(memo => memo.ToContestMemoDto()).ToArray()
    };

    private static ContestMemo ToContestMemoDto(this Domain.ValueObjects.ContestMemo contestMemo) =>
        new(contestMemo.ContestId.Value,
            Enum.Parse<ContestStatus>(contestMemo.Status.ToString()));
}
