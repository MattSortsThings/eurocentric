using ErrorOr;
using Eurocentric.Domain.Aggregates.Broadcasts;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.Aggregates.Broadcasts;

public sealed partial class BroadcastTests : UnitTest
{
    private static readonly ContestYear ContestYear2025 = ContestYear.FromValue(2025).Value;

    private static readonly BroadcastDate BroadcastDate2025May1 =
        BroadcastDate.FromValue(new DateOnly(2025, 5, 1)).Value;

    private static readonly CityName ArbitraryCityName = CityName.FromValue("CityName").Value;
    private static readonly ActName ArbitraryActName = ActName.FromValue("ActName").Value;
    private static readonly SongTitle ArbitrarySongTitle = SongTitle.FromValue("SongTitle").Value;

    private static readonly ContestId FixedContestId = ContestId.FromValue(Guid.Parse("b3ee5f9f-7bf3-4bcf-9a12-c594b105a77f"));

    private static readonly BroadcastId FixedBroadcastId =
        BroadcastId.FromValue(Guid.Parse("eb3036eb-a2b3-450f-b57b-2bfd4516b523"));

    private static readonly CountryId AtId = CountryId.FromValue(Guid.Parse("01abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId BeId = CountryId.FromValue(Guid.Parse("02abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId CzId = CountryId.FromValue(Guid.Parse("03abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId DkId = CountryId.FromValue(Guid.Parse("04abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId EeId = CountryId.FromValue(Guid.Parse("05abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId FiId = CountryId.FromValue(Guid.Parse("06abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId GbId = CountryId.FromValue(Guid.Parse("07abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId HrId = CountryId.FromValue(Guid.Parse("08abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId ItId = CountryId.FromValue(Guid.Parse("09abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId JpId = CountryId.FromValue(Guid.Parse("10abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId KgId = CountryId.FromValue(Guid.Parse("11abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId LuId = CountryId.FromValue(Guid.Parse("12abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId MkId = CountryId.FromValue(Guid.Parse("13abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));
    private static readonly CountryId XxId = CountryId.FromValue(Guid.Parse("24abc7f3-9eea-4703-bb56-e1ac8a3bffa3"));

    private static LiverpoolFormatContest CreateLiverpoolFormatContest(CountryId[] group2CountryIds = null!,
        CountryId[] group1CountryIds = null!,
        CountryId group0CountryId = null!)
    {
        ContestBuilder builder = LiverpoolFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName)
            .AddGroup0Participant(group0CountryId);

        foreach (CountryId countryId in group1CountryIds)
        {
            builder.AddGroup1Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        foreach (CountryId countryId in group2CountryIds)
        {
            builder.AddGroup2Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        ErrorOr<Contest> result = builder.Build(() => FixedContestId);

        return result.Value as LiverpoolFormatContest ?? throw new InvalidCastException("Contest not created");
    }

    private static StockholmFormatContest CreateStockholmFormatContest(CountryId[] group2CountryIds = null!,
        CountryId[] group1CountryIds = null!)
    {
        ContestBuilder builder = StockholmFormatContest.Create()
            .WithContestYear(ContestYear2025)
            .WithCityName(ArbitraryCityName);

        foreach (CountryId countryId in group1CountryIds)
        {
            builder.AddGroup1Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        foreach (CountryId countryId in group2CountryIds)
        {
            builder.AddGroup2Participant(countryId, ArbitraryActName, ArbitrarySongTitle);
        }

        ErrorOr<Contest> result = builder.Build(() => FixedContestId);

        return result.Value as StockholmFormatContest ?? throw new InvalidCastException("Contest not created");
    }

    private static Broadcast CreateSemiFinal1BroadcastWithCompetitors(Contest parentContest,
        params CountryId[] competingCountryIds)
    {
        ErrorOr<Broadcast> result = parentContest.CreateSemiFinal1()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds(competingCountryIds)
            .Build(() => FixedBroadcastId);

        return result.Value;
    }

    private static Broadcast CreateGrandFinalBroadcastWithCompetitors(Contest parentContest,
        params CountryId[] competingCountryIds)
    {
        ErrorOr<Broadcast> result = parentContest.CreateGrandFinal()
            .WithBroadcastDate(BroadcastDate2025May1)
            .WithCompetingCountryIds(competingCountryIds)
            .Build(() => FixedBroadcastId);

        return result.Value;
    }
}
