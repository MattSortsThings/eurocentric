using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Remove_v0_sprocs : Migration
    {
        private const string DownScript1Of2 = "20251019095702_Add_v0_usp_get_broadcast_result_listings.sql";
        private const string DownScript2Of2 =
            "20251019134028_Add_v0_usp_get_competing_country_points_average_rankings.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE v0.usp_get_broadcast_result_listings;");
            migrationBuilder.Sql("DROP PROCEDURE v0.usp_get_competing_country_points_average_rankings;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromScript(DownScript1Of2);
            migrationBuilder.ExecuteSqlFromScript(DownScript2Of2);
        }
    }
}
