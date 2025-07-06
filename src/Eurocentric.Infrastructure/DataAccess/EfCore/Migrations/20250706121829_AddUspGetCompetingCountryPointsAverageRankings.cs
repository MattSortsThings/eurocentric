using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddUspGetCompetingCountryPointsAverageRankings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromEmbeddedResource("20250706121829_AddUspGetCompetingCountryPointsAverageRankings.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string SQL = """
                               DROP PROCEDURE dbo.usp_get_competing_country_points_average_rankings;
                               """;

            migrationBuilder.Sql(SQL);
        }
    }
}
