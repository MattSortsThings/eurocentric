using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryAggregateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "euro");

            migrationBuilder.CreateTable(
                name: "country",
                schema: "euro",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_code = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    country_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_country_country_code", x => x.country_code);
                });

            migrationBuilder.CreateTable(
                name: "country_participating_contest",
                schema: "euro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_status = table.Column<int>(type: "int", nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country_participating_contest", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_country_participating_contest_contest_status_enum", "[contest_status] IN (0, 1, 2)");
                    table.ForeignKey(
                        name: "fk_country_participating_contest_country_country_id",
                        column: x => x.country_id,
                        principalSchema: "euro",
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_country_participating_contest_country_id_contest_id",
                schema: "euro",
                table: "country_participating_contest",
                columns: new[] { "country_id", "contest_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "country_participating_contest",
                schema: "euro");

            migrationBuilder.DropTable(
                name: "country",
                schema: "euro");
        }
    }
}
