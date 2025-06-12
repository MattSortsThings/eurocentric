using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Utilities;

public static class ContestBuilderExtensions
{
    public static ContestBuilder WithArbitraryYearAndCity(this ContestBuilder builder) =>
        builder.WithContestYear(ContestYear.FromValue(2016))
            .WithCityName(CityName.FromValue("CityName"));

    public static ContestBuilder WithGroup1Countries(this ContestBuilder builder, params CountryId[] countryIds)
    {
        ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
        ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

        foreach (CountryId id in countryIds)
        {
            builder.AddGroup1Participant(id, arbitraryActName, arbitrarySongTitle);
        }

        return builder;
    }

    public static ContestBuilder WithGroup2Countries(this ContestBuilder builder, params CountryId[] countries)
    {
        ErrorOr<ActName> arbitraryActName = ActName.FromValue("ActName");
        ErrorOr<SongTitle> arbitrarySongTitle = SongTitle.FromValue("SongTitle");

        foreach (CountryId id in countries)
        {
            builder.AddGroup2Participant(id, arbitraryActName, arbitrarySongTitle);
        }

        return builder;
    }
}
