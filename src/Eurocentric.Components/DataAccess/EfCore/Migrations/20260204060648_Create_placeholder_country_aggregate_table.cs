using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Create_placeholder_country_aggregate_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "placeholder");

            migrationBuilder.CreateTable(
                name: "country",
                schema: "placeholder",
                columns: table => new
                {
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_code = table.Column<string>(
                        type: "char(2)",
                        unicode: false,
                        fixedLength: true,
                        maxLength: 2,
                        nullable: false
                    ),
                    country_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    country_type = table.Column<string>(
                        type: "varchar(6)",
                        unicode: false,
                        maxLength: 6,
                        nullable: false
                    ),
                    contest_ids = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.country_id).Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("AK_country_country_code", x => x.country_code);
                    table.CheckConstraint("CK_country_country_type_Enum", "[country_type] IN ('Real', 'Pseudo')");
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "country", schema: "placeholder");
        }
    }
}
