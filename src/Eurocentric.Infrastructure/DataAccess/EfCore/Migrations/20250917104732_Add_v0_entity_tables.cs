using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_v0_entity_tables : Migration
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
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_date = table.Column<DateOnly>(type: "date", nullable: false),
                    parent_contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<int>(type: "int", nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast", x => x.broadcast_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_broadcast_broadcast_date", x => x.broadcast_date);
                    table.UniqueConstraint("ak_broadcast_parent_contest_id_contest_stage", x => new { x.parent_contest_id, x.contest_stage });
                });

            migrationBuilder.CreateTable(
                name: "contest",
                schema: "v0",
                columns: table => new
                {
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_year = table.Column<int>(type: "int", nullable: false),
                    city_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    contest_rules = table.Column<int>(type: "int", nullable: false),
                    queryable = table.Column<bool>(type: "bit", nullable: false),
                    global_televote_voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest", x => x.contest_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_contest_contest_year", x => x.contest_year);
                });

            migrationBuilder.CreateTable(
                name: "country",
                schema: "v0",
                columns: table => new
                {
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_code = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    country_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country", x => x.country_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_country_country_code", x => x.country_code);
                });

            migrationBuilder.CreateTable(
                name: "competitor",
                schema: "v0",
                columns: table => new
                {
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    running_order_spot = table.Column<int>(type: "int", nullable: false),
                    finishing_position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ak_competitor_broadcast_id_competing_country_id", x => new { x.broadcast_id, x.competing_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_competitor_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "broadcast_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jury",
                schema: "v0",
                columns: table => new
                {
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_awarded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jury", x => new { x.broadcast_id, x.voting_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_jury_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "broadcast_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "televote",
                schema: "v0",
                columns: table => new
                {
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_awarded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_televote", x => new { x.broadcast_id, x.voting_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_televote_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "broadcast_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "child_broadcast",
                schema: "v0",
                columns: table => new
                {
                    child_broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<int>(type: "int", nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_child_broadcast", x => new { x.contest_id, x.child_broadcast_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_child_broadcast_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "participant",
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
                    table.PrimaryKey("pk_participant", x => new { x.contest_id, x.participating_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_participant_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_role",
                schema: "v0",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_role_type = table.Column<int>(type: "int", nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest_role", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_contest_role_country_country_id",
                        column: x => x.country_id,
                        principalSchema: "v0",
                        principalTable: "country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jury_award",
                schema: "v0",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jury_award", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_jury_award_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalSchema: "v0",
                        principalTable: "competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "televote_award",
                schema: "v0",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_televote_award", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "fk_televote_award_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalSchema: "v0",
                        principalTable: "competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_child_broadcast_contest_id_contest_stage",
                schema: "v0",
                table: "child_broadcast",
                columns: new[] { "contest_id", "contest_stage" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_competitor_broadcast_id_running_order_spot",
                schema: "v0",
                table: "competitor",
                columns: new[] { "broadcast_id", "running_order_spot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contest_role_country_id_contest_id",
                schema: "v0",
                table: "contest_role",
                columns: new[] { "country_id", "contest_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_jury_award_broadcast_id_competing_country_id_voting_country_id",
                schema: "v0",
                table: "jury_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_televote_award_broadcast_id_competing_country_id_voting_country_id",
                schema: "v0",
                table: "televote_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "child_broadcast",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "contest_role",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "jury",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "jury_award",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "participant",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "televote",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "televote_award",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "country",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "contest",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "competitor",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast",
                schema: "v0");
        }
    }
}
