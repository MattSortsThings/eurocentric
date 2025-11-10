using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_udf_get_contest_stage_sort_order_value : Migration
    {
        private const string UpScript = "20251109174201_Add_dbo_udf_get_contest_stage_sort_order_value.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromScript(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION dbo.udf_get_contest_stage_sort_order_value;");
        }
    }
}
