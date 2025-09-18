using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_v0_usp_get_competing_country_points_in_range_rankings : Migration
    {
        private const string UpScript =
            "Eurocentric.Infrastructure.DataAccess.Scripts.add_v0_usp_get_competing_country_points_in_range_rankings.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromEmbeddedResource(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string sql = "DROP PROCEDURE v0.usp_get_competing_country_points_in_range_rankings;";

            migrationBuilder.Sql(sql);
        }
    }
}
