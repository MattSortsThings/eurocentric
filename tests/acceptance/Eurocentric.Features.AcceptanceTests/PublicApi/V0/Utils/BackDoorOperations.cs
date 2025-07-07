using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eurocentric.Features.AcceptanceTests.PublicApi.V0.Utils;

internal static class BackDoorOperations
{
    internal static async Task PopulateSampleQueryableCountriesAsync(IServiceProvider serviceProvider)
    {
        await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        const string sql = """
                           insert into dbo.placeholder_queryable_country (country_code, country_name)
                           values  (N'AT', N'Austria'),
                                   (N'BE', N'Belgium'),
                                   (N'CZ', N'Czechia'),
                                   (N'DK', N'Denmark'),
                                   (N'EE', N'Estonia'),
                                   (N'FI', N'Finland'),
                                   (N'GE', N'Georgia');
                           """;
        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    internal static async Task PopulateSampleQueryableJuryAwardsAsync(IServiceProvider serviceProvider)
    {
        await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        const string sql = """
                           insert into dbo.placeholder_queryable_jury_award (contest_year,
                           contest_stage,
                           competing_country_code,
                           voting_country_code,
                           broadcast_tag,
                           running_order_position,
                           points_value,
                           max_points_value,
                           real_points_value,
                           normalized_points_value)
                           values  (2021, 0, N'AT', N'BE', N'2021-0', 1, 10, 12, 10, 0.84),
                                   (2021, 0, N'AT', N'CZ', N'2021-0', 1, 12, 12, 12, 1),
                                   (2021, 0, N'AT', N'DK', N'2021-0', 1, 8, 12, 8, 0.68),
                                   (2021, 0, N'AT', N'EE', N'2021-0', 1, 12, 12, 12, 1),
                                   (2021, 0, N'BE', N'AT', N'2021-0', 2, 12, 12, 12, 1),
                                   (2021, 0, N'BE', N'CZ', N'2021-0', 2, 8, 12, 8, 0.68),
                                   (2021, 0, N'BE', N'DK', N'2021-0', 2, 12, 12, 12, 1),
                                   (2021, 0, N'BE', N'EE', N'2021-0', 2, 10, 12, 10, 0.84),
                                   (2021, 0, N'CZ', N'AT', N'2021-0', 3, 10, 12, 10, 0.84),
                                   (2021, 0, N'CZ', N'BE', N'2021-0', 3, 8, 12, 8, 0.68),
                                   (2021, 0, N'CZ', N'DK', N'2021-0', 3, 10, 12, 10, 0.84),
                                   (2021, 0, N'CZ', N'EE', N'2021-0', 3, 0, 12, 0, 0.04),
                                   (2021, 0, N'DK', N'AT', N'2021-0', 4, 8, 12, 8, 0.68),
                                   (2021, 0, N'DK', N'BE', N'2021-0', 4, 12, 12, 12, 1),
                                   (2021, 0, N'DK', N'CZ', N'2021-0', 4, 10, 12, 10, 0.84),
                                   (2021, 0, N'DK', N'EE', N'2021-0', 4, 8, 12, 8, 0.68),
                                   (2022, 0, N'AT', N'BE', N'2022-0', 1, 8, 12, 8, 0.68),
                                   (2022, 0, N'AT', N'CZ', N'2022-0', 1, 8, 12, 8, 0.68),
                                   (2022, 0, N'AT', N'DK', N'2022-0', 1, 10, 12, 10, 0.84),
                                   (2022, 0, N'AT', N'EE', N'2022-0', 1, 0, 12, 0, 0.04),
                                   (2022, 0, N'AT', N'FI', N'2022-0', 1, 12, 12, 12, 1),
                                   (2022, 0, N'BE', N'AT', N'2022-0', 2, 12, 12, 12, 1),
                                   (2022, 0, N'BE', N'CZ', N'2022-0', 2, 12, 12, 12, 1),
                                   (2022, 0, N'BE', N'DK', N'2022-0', 2, 12, 12, 12, 1),
                                   (2022, 0, N'BE', N'EE', N'2022-0', 2, 10, 12, 10, 0.84),
                                   (2022, 0, N'BE', N'FI', N'2022-0', 2, 10, 12, 10, 0.84),
                                   (2022, 0, N'CZ', N'AT', N'2022-0', 3, 10, 12, 10, 0.84),
                                   (2022, 0, N'CZ', N'BE', N'2022-0', 3, 12, 12, 12, 1),
                                   (2022, 0, N'CZ', N'DK', N'2022-0', 3, 8, 12, 8, 0.68),
                                   (2022, 0, N'CZ', N'EE', N'2022-0', 3, 8, 12, 8, 0.68),
                                   (2022, 0, N'CZ', N'FI', N'2022-0', 3, 8, 12, 8, 0.68),
                                   (2022, 0, N'DK', N'AT', N'2022-0', 4, 8, 12, 8, 0.68),
                                   (2022, 0, N'DK', N'BE', N'2022-0', 4, 10, 12, 10, 0.84),
                                   (2022, 0, N'DK', N'CZ', N'2022-0', 4, 10, 12, 10, 0.84),
                                   (2022, 0, N'DK', N'EE', N'2022-0', 4, 12, 12, 12, 1),
                                   (2022, 0, N'DK', N'FI', N'2022-0', 4, 0, 12, 0, 0.04);
                           """;

        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }

    internal static async Task PopulateSampleQueryableTelevoteAwardsAsync(IServiceProvider serviceProvider)
    {
        await using AppDbContext dbContext = serviceProvider.GetRequiredService<AppDbContext>();

        const string sql = """
                           insert into dbo.placeholder_queryable_televote_award (contest_year,
                           contest_stage,
                           competing_country_code,
                           voting_country_code,
                           broadcast_tag,
                           running_order_position,
                           points_value,
                           max_points_value,
                           real_points_value,
                           normalized_points_value)
                           values  (2021, 0, N'AT', N'BE', N'2021-0', 1, 8, 12, 8, 0.68),
                                   (2021, 0, N'AT', N'CZ', N'2021-0', 1, 12, 12, 12, 1),
                                   (2021, 0, N'AT', N'DK', N'2021-0', 1, 12, 12, 12, 1),
                                   (2021, 0, N'AT', N'EE', N'2021-0', 1, 0, 12, 0, 0.04),
                                   (2021, 0, N'BE', N'AT', N'2021-0', 2, 12, 12, 12, 1),
                                   (2021, 0, N'BE', N'CZ', N'2021-0', 2, 10, 12, 10, 0.84),
                                   (2021, 0, N'BE', N'DK', N'2021-0', 2, 10, 12, 10, 0.84),
                                   (2021, 0, N'BE', N'EE', N'2021-0', 2, 8, 12, 8, 0.68),
                                   (2021, 0, N'CZ', N'AT', N'2021-0', 3, 10, 12, 10, 0.84),
                                   (2021, 0, N'CZ', N'BE', N'2021-0', 3, 12, 12, 12, 1),
                                   (2021, 0, N'CZ', N'DK', N'2021-0', 3, 8, 12, 8, 0.68),
                                   (2021, 0, N'CZ', N'EE', N'2021-0', 3, 10, 12, 10, 0.84),
                                   (2021, 0, N'DK', N'AT', N'2021-0', 4, 8, 12, 8, 0.68),
                                   (2021, 0, N'DK', N'BE', N'2021-0', 4, 10, 12, 10, 0.84),
                                   (2021, 0, N'DK', N'CZ', N'2021-0', 4, 8, 12, 8, 0.68),
                                   (2021, 0, N'DK', N'EE', N'2021-0', 4, 12, 12, 12, 1),
                                   (2022, 0, N'AT', N'BE', N'2022-0', 1, 8, 12, 8, 0.68),
                                   (2022, 0, N'AT', N'CZ', N'2022-0', 1, 12, 12, 12, 1),
                                   (2022, 0, N'AT', N'DK', N'2022-0', 1, 12, 12, 12, 1),
                                   (2022, 0, N'AT', N'EE', N'2022-0', 1, 0, 12, 0, 0.04),
                                   (2022, 0, N'AT', N'FI', N'2022-0', 1, 12, 12, 12, 1),
                                   (2022, 0, N'BE', N'AT', N'2022-0', 2, 12, 12, 12, 1),
                                   (2022, 0, N'BE', N'CZ', N'2022-0', 2, 10, 12, 10, 0.84),
                                   (2022, 0, N'BE', N'DK', N'2022-0', 2, 10, 12, 10, 0.84),
                                   (2022, 0, N'BE', N'EE', N'2022-0', 2, 8, 12, 8, 0.68),
                                   (2022, 0, N'BE', N'FI', N'2022-0', 2, 10, 12, 10, 0.84),
                                   (2022, 0, N'CZ', N'AT', N'2022-0', 3, 10, 12, 10, 0.84),
                                   (2022, 0, N'CZ', N'BE', N'2022-0', 3, 12, 12, 12, 1),
                                   (2022, 0, N'CZ', N'DK', N'2022-0', 3, 8, 12, 8, 0.68),
                                   (2022, 0, N'CZ', N'EE', N'2022-0', 3, 10, 12, 10, 0.84),
                                   (2022, 0, N'CZ', N'FI', N'2022-0', 3, 8, 12, 8, 0.68),
                                   (2022, 0, N'DK', N'AT', N'2022-0', 4, 8, 12, 8, 0.68),
                                   (2022, 0, N'DK', N'BE', N'2022-0', 4, 10, 12, 10, 0.84),
                                   (2022, 0, N'DK', N'CZ', N'2022-0', 4, 8, 12, 8, 0.68),
                                   (2022, 0, N'DK', N'EE', N'2022-0', 4, 12, 12, 12, 1),
                                   (2022, 0, N'DK', N'FI', N'2022-0', 4, 0, 12, 0, 0.04);
                           """;

        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }
}
