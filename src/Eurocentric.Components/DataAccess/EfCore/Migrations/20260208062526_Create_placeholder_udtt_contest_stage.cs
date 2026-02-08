using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Create_placeholder_udtt_contest_stage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string sql = """
                CREATE TYPE [placeholder].[udtt_contest_stage] AS TABLE
                (
                  [contest_stage] VARCHAR(10) NOT NULL CHECK ([contest_stage] IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal'))
                );
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string sql = "DROP TYPE [placeholder].[udtt_contest_stage];";

            migrationBuilder.Sql(sql);
        }
    }
}
