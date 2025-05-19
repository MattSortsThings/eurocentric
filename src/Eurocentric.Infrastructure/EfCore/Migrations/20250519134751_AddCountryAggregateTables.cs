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
            migrationBuilder.CreateTable(
                name: "country",
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
                name: "country_contest_memo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country_contest_memo", x => x.id);
                    table.CheckConstraint("ck_country_contest_memo_contest_status_enum", "[contest_status] IN (N'Initialized', N'InProgress', N'Completed')");
                    table.ForeignKey(
                        name: "fk_country_contest_memo_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_country_contest_memo_country_id_contest_id",
                table: "country_contest_memo",
                columns: ["country_id", "contest_id"],
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "country_contest_memo");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
