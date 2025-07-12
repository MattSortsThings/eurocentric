using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddContestAggregateTables : Migration
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
                    completed = table.Column<bool>(type: "bit", nullable: false),
                    contest_format = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_contest_contest_year", x => x.contest_year);
                    table.CheckConstraint("ck_contest_contest_format_enum", "[contest_format] IN (0,1)");
                });

            migrationBuilder.CreateTable(
                name: "contest_child_broadcast",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<int>(type: "int", nullable: false),
                    broadcast_status = table.Column<int>(type: "int", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest_child_broadcast", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_contest_child_broadcast_broadcast_status_enum", "[broadcast_status] IN (0,1,2)");
                    table.CheckConstraint("ck_contest_child_broadcast_contest_stage_enum", "[contest_stage] IN (0,1,2)");
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
                    table.CheckConstraint("ck_contest_participant_nullability_permutations", "([participant_group] = 0 AND [act_name] IS NULL AND [song_title] IS NULL) OR ([participant_group] <> 0 AND [act_name] IS NOT NULL AND [song_title] IS NOT NULL)");
                    table.CheckConstraint("ck_contest_participant_participant_group_enum", "[participant_group] IN (0,1,2)");
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
