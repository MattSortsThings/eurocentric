using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_domain_aggregate_types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contest",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_year = table.Column<int>(type: "int", nullable: false),
                    city_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    contest_format = table.Column<int>(type: "int", nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_contest_contest_year", x => x.contest_year);
                    table.CheckConstraint("ck_contest_contest_format_enum_value", "[contest_format] BETWEEN 0 AND 1");
                });

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
                name: "contest_child_broadcast",
                columns: table => new
                {
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<int>(type: "int", nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest_child_broadcast", x => new { x.contest_id, x.broadcast_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_contest_child_broadcast_contest_stage_enum_value", "[contest_stage] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "fk_contest_child_broadcast_contest_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_participant",
                columns: table => new
                {
                    participating_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    participant_group = table.Column<int>(type: "int", nullable: false),
                    act_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    song_title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest_participant", x => new { x.contest_id, x.participating_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_contest_participant_participant_group_enum_value", "[participant_group] BETWEEN 0 AND 2");
                    table.CheckConstraint("ck_contest_participant_permitted_nullability", "([participant_group] = 0 AND [act_name] IS NULL AND [song_title] IS NULL) OR ([participant_group] <> 0 AND [act_name] IS NOT NULL AND [song_title] IS NOT NULL)");
                    table.ForeignKey(
                        name: "fk_contest_participant_contest_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "country_participating_contest",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    participating_contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country_participating_contest", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_country_participating_contest_country_country_id",
                        column: x => x.country_id,
                        principalTable: "country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contest_child_broadcast_contest_id_contest_stage",
                table: "contest_child_broadcast",
                columns: new[] { "contest_id", "contest_stage" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_country_participating_contest_country_id",
                table: "country_participating_contest",
                column: "country_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contest_child_broadcast");

            migrationBuilder.DropTable(
                name: "contest_participant");

            migrationBuilder.DropTable(
                name: "country_participating_contest");

            migrationBuilder.DropTable(
                name: "contest");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
