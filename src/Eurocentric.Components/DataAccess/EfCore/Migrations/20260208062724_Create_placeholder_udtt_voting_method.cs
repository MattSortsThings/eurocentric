using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Create_placeholder_udtt_voting_method : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string sql = """
                CREATE TYPE [placeholder].[udtt_voting_method] AS TABLE
                (
                  [voting_method] VARCHAR(8) NOT NULL CHECK ([voting_method] IN ('Televote', 'Jury'))
                );
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string sql = "DROP TYPE [placeholder].[udtt_voting_method];";

            migrationBuilder.Sql(sql);
        }
    }
}
