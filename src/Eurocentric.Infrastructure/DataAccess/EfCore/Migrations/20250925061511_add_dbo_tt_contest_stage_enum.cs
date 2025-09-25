using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class add_dbo_tt_contest_stage_enum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string sql = """
                CREATE TYPE dbo.tt_contest_stage_enum AS TABLE
                (
                    contest_stage NVARCHAR(10) NOT NULL CHECK (contest_stage IN (N'SemiFinal1',
                                                                                 N'SemiFinal2',
                                                                                 N'GrandFinal'))
                );
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TYPE dbo.tt_contest_stage_enum;");
        }
    }
}
