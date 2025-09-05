using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_contest_aggregate_tables : Migration
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
                    completed = table.Column<bool>(type: "bit", nullable: false),
                    global_televote_participating_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_contest_contest_year", x => x.contest_year);
                    table.CheckConstraint("ck_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050");
                    table.CheckConstraint("ck_contest_global_televote_nullability", "([contest_format] = 0 AND [global_televote_participating_country_id] IS NOT NULL) OR ([contest_format] = 1 AND [global_televote_participating_country_id] IS NULL)");
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
                    table.CheckConstraint("ck_contest_child_broadcast_contest_stage_enum", "[contest_stage] BETWEEN 0 AND 2");
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
                    semi_final_draw = table.Column<int>(type: "int", nullable: false),
                    act_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    song_title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest_participant", x => new { x.contest_id, x.participating_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_contest_participant_semi_final_draw_enum", "[semi_final_draw] BETWEEN 0 AND 1");
                    table.ForeignKey(
                        name: "fk_contest_participant_contest_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contest_child_broadcast_contest_id_contest_stage",
                table: "contest_child_broadcast",
                columns: new[] { "contest_id", "contest_stage" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contest_child_broadcast");

            migrationBuilder.DropTable(
                name: "contest_participant");

            migrationBuilder.DropTable(
                name: "contest");
        }
    }
}
