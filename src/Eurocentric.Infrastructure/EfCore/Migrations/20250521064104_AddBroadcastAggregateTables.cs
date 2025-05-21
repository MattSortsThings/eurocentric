using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddBroadcastAggregateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "broadcast",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    broadcast_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast", x => x.id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_broadcast_contest_id_contest_stage", x => new { x.contest_id, x.contest_stage });
                    table.CheckConstraint("ck_broadcast_broadcast_status_enum", "[broadcast_status] IN (N'Initialized', N'InProgress', N'Completed')");
                    table.CheckConstraint("ck_broadcast_contest_stage_enum", "[contest_stage] IN (N'SemiFinal1', N'SemiFinal2', N'GrandFinal')");
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor",
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
                    table.CheckConstraint("ck_broadcast_competitor_finishing_position", "[finishing_position] > 0");
                    table.CheckConstraint("ck_broadcast_competitor_running_order_position", "[running_order_position] > 0");
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalTable: "broadcast",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_jury",
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
                        principalTable: "broadcast",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_televote",
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
                        principalTable: "broadcast",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor_jury_award",
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
                    table.CheckConstraint("ck_broadcast_competitor_jury_award_country_ids", "[competing_country_id] <> [voting_country_id]");
                    table.CheckConstraint("ck_broadcast_competitor_jury_award_points_value_enum", "[points_value] IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12)");
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_jury_award_broadcast_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalTable: "broadcast_competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor_televote_award",
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
                    table.CheckConstraint("ck_broadcast_competitor_televote_award_country_ids", "[competing_country_id] <> [voting_country_id]");
                    table.CheckConstraint("ck_broadcast_competitor_televote_award_points_value_enum", "[points_value] IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12)");
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_televote_award_broadcast_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalTable: "broadcast_competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_broadcast_id_running_order_position",
                table: "broadcast_competitor",
                columns: new[] { "broadcast_id", "running_order_position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_jury_award_broadcast_id_competing_country_id_voting_country_id",
                table: "broadcast_competitor_jury_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_televote_award_broadcast_id_competing_country_id_voting_country_id",
                table: "broadcast_competitor_televote_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "broadcast_competitor_jury_award");

            migrationBuilder.DropTable(
                name: "broadcast_competitor_televote_award");

            migrationBuilder.DropTable(
                name: "broadcast_jury");

            migrationBuilder.DropTable(
                name: "broadcast_televote");

            migrationBuilder.DropTable(
                name: "broadcast_competitor");

            migrationBuilder.DropTable(
                name: "broadcast");
        }
    }
}
