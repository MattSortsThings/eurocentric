using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Remove_v0_usp_get_competing_country_points_average_rankings : Migration
    {
        private const string DownScript = "20250731073451_Add_v0_usp_get_competing_country_points_average_rankings.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE v0.usp_get_competing_country_points_average_rankings;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromLocalScript(DownScript);
        }
    }
}
