using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_usp_get_competitor_points_average_rankings : Migration
    {
        private const string UpScript = "20251110090751_Add_dbo_usp_get_competitor_points_average_rankings.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromScript(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.usp_get_competitor_points_average_rankings;");
        }
    }
}
