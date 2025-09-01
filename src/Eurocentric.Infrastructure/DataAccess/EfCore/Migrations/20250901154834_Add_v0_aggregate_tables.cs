using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_v0_aggregate_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "v0");

            migrationBuilder.CreateTable(
                name: "broadcast",
                schema: "v0",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_date = table.Column<DateOnly>(type: "date", nullable: false),
                    parent_contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<int>(type: "int", nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_broadcast_broadcast_date", x => x.broadcast_date);
                    table.UniqueConstraint("ak_broadcast_parent_contest_id_contest_stage", x => new { x.parent_contest_id, x.contest_stage });
                });

            migrationBuilder.CreateTable(
                name: "contest",
                schema: "v0",
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
                });

            migrationBuilder.CreateTable(
                name: "country",
                schema: "v0",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_code = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    country_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    participating_contest_ids = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_country_country_code", x => x.country_code);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor",
                schema: "v0",
                columns: table => new
                {
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    running_order_position = table.Column<int>(type: "int", nullable: false),
                    finishing_position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ak_broadcast_competitor_broadcast_id_competing_country_id", x => new { x.broadcast_id, x.competing_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_jury",
                schema: "v0",
                columns: table => new
                {
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_awarded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast_jury", x => new { x.broadcast_id, x.voting_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_broadcast_jury_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_televote",
                schema: "v0",
                columns: table => new
                {
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_awarded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast_televote", x => new { x.broadcast_id, x.voting_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_broadcast_televote_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_child_broadcast",
                schema: "v0",
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
                    table.ForeignKey(
                        name: "fk_contest_child_broadcast_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_participant",
                schema: "v0",
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
                    table.ForeignKey(
                        name: "fk_contest_participant_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor_jury_award",
                schema: "v0",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast_competitor_jury_award", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_jury_award_broadcast_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalSchema: "v0",
                        principalTable: "broadcast_competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor_televote_award",
                schema: "v0",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast_competitor_televote_award", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_televote_award_broadcast_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalSchema: "v0",
                        principalTable: "broadcast_competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_broadcast_id_running_order_position",
                schema: "v0",
                table: "broadcast_competitor",
                columns: new[] { "broadcast_id", "running_order_position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_jury_award_broadcast_id",
                schema: "v0",
                table: "broadcast_competitor_jury_award",
                column: "broadcast_id");

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_jury_award_broadcast_id_competing_country_id_voting_country_id",
                schema: "v0",
                table: "broadcast_competitor_jury_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_jury_award_competing_country_id",
                schema: "v0",
                table: "broadcast_competitor_jury_award",
                column: "competing_country_id");

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_jury_award_voting_country_id",
                schema: "v0",
                table: "broadcast_competitor_jury_award",
                column: "voting_country_id");

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_televote_award_broadcast_id",
                schema: "v0",
                table: "broadcast_competitor_televote_award",
                column: "broadcast_id");

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_televote_award_broadcast_id_competing_country_id_voting_country_id",
                schema: "v0",
                table: "broadcast_competitor_televote_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_televote_award_competing_country_id",
                schema: "v0",
                table: "broadcast_competitor_televote_award",
                column: "competing_country_id");

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_televote_award_voting_country_id",
                schema: "v0",
                table: "broadcast_competitor_televote_award",
                column: "voting_country_id");

            migrationBuilder.CreateIndex(
                name: "ix_contest_child_broadcast_contest_id_contest_stage",
                schema: "v0",
                table: "contest_child_broadcast",
                columns: new[] { "contest_id", "contest_stage" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "broadcast_competitor_jury_award",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast_competitor_televote_award",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast_jury",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast_televote",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "contest_child_broadcast",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "contest_participant",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "country",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast_competitor",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "contest",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast",
                schema: "v0");
        }
    }
}
