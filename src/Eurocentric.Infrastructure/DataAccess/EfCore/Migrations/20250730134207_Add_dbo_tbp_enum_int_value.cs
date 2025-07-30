using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_tbp_enum_int_value : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string SQL = """
                               CREATE TYPE dbo.tvp_enum_int_value AS TABLE
                               (
                                   value INT NOT NULL
                               );
                               """;

            migrationBuilder.Sql(SQL);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string SQL = """
                               DROP TYPE dbo.tvp_enum_int_value;
                               """;

            migrationBuilder.Sql(SQL);
        }
    }
}
