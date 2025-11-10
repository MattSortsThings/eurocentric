using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_udf_normalize_points_value : Migration
    {
        private const string UpScript = "20251109175010_Add_dbo_udf_normalize_points_value.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromScript(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION dbo.udf_normalize_points_value;");
        }
    }
}
