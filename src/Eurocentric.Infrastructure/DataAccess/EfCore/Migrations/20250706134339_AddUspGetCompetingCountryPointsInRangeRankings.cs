using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddUspGetCompetingCountryPointsInRangeRankings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromEmbeddedResource("20250706134339_AddUspGetCompetingCountryPointsInRangeRankings.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string SQL = """
                               DROP PROCEDURE dbo.usp_get_competing_country_points_in_range_rankings;
                               """;

            migrationBuilder.Sql(SQL);
        }
    }
}
