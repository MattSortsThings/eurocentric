using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_v0_broadcast_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    table.UniqueConstraint("ak_broadcast_parent_contest_id_contest_stage", x => new { x.parent_contest_id, x.contest_stage });
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor",
                schema: "v0",
                columns: table => new
                {
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    finishing_position = table.Column<int>(type: "int", nullable: false),
                    running_order_position = table.Column<int>(type: "int", nullable: false)
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
                name: "broadcast_competitor_jury_award",
                schema: "v0",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "ix_broadcast_broadcast_date",
                schema: "v0",
                table: "broadcast",
                column: "broadcast_date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_broadcast_id_running_order_position",
                schema: "v0",
                table: "broadcast_competitor",
                columns: new[] { "broadcast_id", "running_order_position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_jury_award_broadcast_id_competing_country_id",
                schema: "v0",
                table: "broadcast_competitor_jury_award",
                columns: new[] { "broadcast_id", "competing_country_id" });

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_televote_award_broadcast_id_competing_country_id",
                schema: "v0",
                table: "broadcast_competitor_televote_award",
                columns: new[] { "broadcast_id", "competing_country_id" });
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
                name: "broadcast_competitor",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast",
                schema: "v0");
        }
    }
}
