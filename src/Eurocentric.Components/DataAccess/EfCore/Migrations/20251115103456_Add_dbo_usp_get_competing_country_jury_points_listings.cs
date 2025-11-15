using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_usp_get_competing_country_jury_points_listings : Migration
    {
        private const string UpScript = "20251115103456_Add_dbo_usp_get_competing_country_jury_points_listings.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromScript(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.usp_get_competing_country_jury_points_listings;");
        }
    }
}
