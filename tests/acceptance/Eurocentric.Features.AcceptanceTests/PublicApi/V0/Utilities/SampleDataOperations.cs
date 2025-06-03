using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Eurocentric.Domain.Enums;
using Eurocentric.Domain.Placeholders;
using Eurocentric.Infrastructure.InMemoryRepositories;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utilities;

public static class SampleDataOperations
{
    private const string DateFormat = "yyyy-MM-dd";

    public static void PopulateSampleCountries(IServiceProvider serviceProvider)
    {
        InMemoryQueryableRepository repository = serviceProvider.GetRequiredService<InMemoryQueryableRepository>();

        List<QueryableCountry> countries =
            ReadFromCsvEmbeddedResource<QueryableCountry, QueryableCountryClassMap>(Paths.Countries);

        repository.QueryableCountries.AddRange(countries);
    }

    public static void PopulateTurin2022Contest(IServiceProvider serviceProvider)
    {
        InMemoryQueryableRepository repository = serviceProvider.GetRequiredService<InMemoryQueryableRepository>();

        List<QueryableCompetitor> competitors =
            ReadFromCsvEmbeddedResource<QueryableCompetitor, QueryableCompetitorClassMap>(Paths.Turin2022.Competitors);

        List<QueryablePointsAward> juryAwards =
            ReadFromCsvEmbeddedResource<QueryablePointsAward, QueryablePointsAwardClassMap>(Paths.Turin2022.JuryAwards);

        List<QueryablePointsAward> televoteAwards =
            ReadFromCsvEmbeddedResource<QueryablePointsAward, QueryablePointsAwardClassMap>(Paths.Turin2022.TelevoteAwards);

        repository.QueryableCompetitors.AddRange(competitors);
        repository.QueryableJuryPointsAwards.AddRange(juryAwards);
        repository.QueryableTelevotePointsAwards.AddRange(televoteAwards);

        repository.QueryableContests.Add(new QueryableContest { ContestYear = 2022, CityName = "Turin" });

        repository.QueryableBroadcasts.Add(new QueryableBroadcast
        {
            ContestYear = 2022,
            ContestStage = ContestStage.SemiFinal1,
            BroadcastDate = DateOnly.ParseExact("2022-05-10", DateFormat),
            TelevoteOnly = false
        });

        repository.QueryableBroadcasts.Add(new QueryableBroadcast
        {
            ContestYear = 2022,
            ContestStage = ContestStage.SemiFinal2,
            BroadcastDate = DateOnly.ParseExact("2022-05-12", DateFormat),
            TelevoteOnly = false
        });

        repository.QueryableBroadcasts.Add(new QueryableBroadcast
        {
            ContestYear = 2022,
            ContestStage = ContestStage.GrandFinal,
            BroadcastDate = DateOnly.ParseExact("2022-05-14", DateFormat),
            TelevoteOnly = false
        });
    }

    public static void PopulateLiverpool2023Contest(IServiceProvider serviceProvider)
    {
        InMemoryQueryableRepository repository = serviceProvider.GetRequiredService<InMemoryQueryableRepository>();

        List<QueryableCompetitor> competitors =
            ReadFromCsvEmbeddedResource<QueryableCompetitor, QueryableCompetitorClassMap>(Paths.Liverpool2023.Competitors);

        List<QueryablePointsAward> juryAwards =
            ReadFromCsvEmbeddedResource<QueryablePointsAward, QueryablePointsAwardClassMap>(Paths.Liverpool2023.JuryAwards);

        List<QueryablePointsAward> televoteAwards =
            ReadFromCsvEmbeddedResource<QueryablePointsAward, QueryablePointsAwardClassMap>(Paths.Liverpool2023.TelevoteAwards);

        repository.QueryableCompetitors.AddRange(competitors);
        repository.QueryableJuryPointsAwards.AddRange(juryAwards);
        repository.QueryableTelevotePointsAwards.AddRange(televoteAwards);

        repository.QueryableContests.Add(new QueryableContest { ContestYear = 2023, CityName = "Liverpool" });

        repository.QueryableBroadcasts.Add(new QueryableBroadcast
        {
            ContestYear = 2023,
            ContestStage = ContestStage.SemiFinal1,
            BroadcastDate = DateOnly.ParseExact("2023-05-09", DateFormat),
            TelevoteOnly = true
        });

        repository.QueryableBroadcasts.Add(new QueryableBroadcast
        {
            ContestYear = 2023,
            ContestStage = ContestStage.SemiFinal2,
            BroadcastDate = DateOnly.ParseExact("2022-05-11", DateFormat),
            TelevoteOnly = true
        });

        repository.QueryableBroadcasts.Add(new QueryableBroadcast
        {
            ContestYear = 2023,
            ContestStage = ContestStage.GrandFinal,
            BroadcastDate = DateOnly.ParseExact("2022-05-13", DateFormat),
            TelevoteOnly = false
        });
    }

    private static List<TRecord> ReadFromCsvEmbeddedResource<TRecord, TMapper>(string path)
        where TMapper : ClassMap<TRecord>
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream(path);
        using StreamReader reader = new(stream!);

        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<TMapper>();

        return csv.GetRecords<TRecord>().ToList();
    }

    private static class Paths
    {
        public const string Countries =
            "Eurocentric.Features.AcceptanceTests.Resources.PublicApiV0SampleData.countries.csv";

        public static class Turin2022
        {
            public const string Competitors =
                "Eurocentric.Features.AcceptanceTests.Resources.PublicApiV0SampleData.turin_2022_competitors.csv";
            public const string JuryAwards =
                "Eurocentric.Features.AcceptanceTests.Resources.PublicApiV0SampleData.turin_2022_jury_awards.csv";
            public const string TelevoteAwards =
                "Eurocentric.Features.AcceptanceTests.Resources.PublicApiV0SampleData.turin_2022_televote_awards.csv";
        }

        public static class Liverpool2023
        {
            public const string Competitors =
                "Eurocentric.Features.AcceptanceTests.Resources.PublicApiV0SampleData.liverpool_2023_competitors.csv";
            public const string JuryAwards =
                "Eurocentric.Features.AcceptanceTests.Resources.PublicApiV0SampleData.liverpool_2023_jury_awards.csv";
            public const string TelevoteAwards =
                "Eurocentric.Features.AcceptanceTests.Resources.PublicApiV0SampleData.liverpool_2023_televote_awards.csv";
        }
    }

    [UsedImplicitly]
    private sealed class QueryableCountryClassMap : ClassMap<QueryableCountry>
    {
        public QueryableCountryClassMap()
        {
            Map(country => country.CountryCode).Name("country_code");
            Map(country => country.CountryName).Name("country_name");
        }
    }

    [UsedImplicitly]
    private sealed class QueryableCompetitorClassMap : ClassMap<QueryableCompetitor>
    {
        public QueryableCompetitorClassMap()
        {
            Map(competitor => competitor.ContestYear).Name("contest_year");
            Map(competitor => competitor.ContestStage).Name("contest_stage")
                .TypeConverter(new EnumConverter(typeof(ContestStage)));
            Map(competitor => competitor.CompetingCountryCode).Name("competing_country_code");
            Map(competitor => competitor.RunningOrderPosition).Name("running_order_position");
            Map(competitor => competitor.ActName).Name("act_name");
            Map(competitor => competitor.SongTitle).Name("song_title");
            Map(competitor => competitor.FinishingPosition).Name("finishing_position");
        }
    }

    [UsedImplicitly]
    private sealed class QueryablePointsAwardClassMap : ClassMap<QueryablePointsAward>
    {
        public QueryablePointsAwardClassMap()
        {
            Map(award => award.ContestYear).Name("contest_year");
            Map(award => award.ContestStage).Name("contest_stage").TypeConverter(new EnumConverter(typeof(ContestStage)));
            Map(award => award.CompetingCountryCode).Name("competing_country_code");
            Map(award => award.VotingCountryCode).Name("voting_country_code");
            Map(award => award.PointsValue).Name("points_value");
            Map(award => award.MaxPointsValue).Name("max_points_value");
        }
    }
}
