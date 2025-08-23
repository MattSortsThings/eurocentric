using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_usp_get_competing_country_points_in_range_rankings : Migration
    {
        private const string UpScript = "20250822140336_Add_dbo_usp_get_competing_country_points_in_range_rankings.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromLocalScript(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.usp_get_competing_country_points_in_range_rankings;");
        }
    }
}
