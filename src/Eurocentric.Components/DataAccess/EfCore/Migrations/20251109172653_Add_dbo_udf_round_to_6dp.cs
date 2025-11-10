using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_udf_round_to_6dp : Migration
    {
        private const string UpScript = "20251109172653_Add_dbo_udf_round_to_6dp.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromScript(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION dbo.udf_round_to_6dp;");
        }
    }
}
