using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Create_v0_broadcast_aggregate_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "broadcast",
                schema: "v0",
                columns: table => new
                {
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_date = table.Column<DateOnly>(type: "date", nullable: false),
                    parent_contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<string>(
                        type: "varchar(10)",
                        unicode: false,
                        maxLength: 10,
                        nullable: false
                    ),
                    broadcast_format = table.Column<string>(
                        type: "varchar(15)",
                        unicode: false,
                        maxLength: 15,
                        nullable: false
                    ),
                    completed = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_broadcast", x => x.broadcast_id).Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("AK_broadcast_broadcast_date", x => x.broadcast_date);
                    table.UniqueConstraint(
                        "AK_broadcast_parent_contest_id_contest_stage",
                        x => new { x.parent_contest_id, x.contest_stage }
                    );
                    table.CheckConstraint(
                        "CK_broadcast_broadcast_date",
                        "broadcast_date BETWEEN '2016-01-01' AND '2030-12-31'"
                    );
                    table.CheckConstraint(
                        "CK_broadcast_broadcast_format_Enum",
                        "[broadcast_format] IN ('JuryAndTelevote', 'TelevoteOnly')"
                    );
                    table.CheckConstraint(
                        "CK_broadcast_contest_stage_Enum",
                        "[contest_stage] IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "broadcast_competitor",
                schema: "v0",
                columns: table => new
                {
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    performing_spot = table.Column<int>(type: "int", nullable: false),
                    broadcast_half = table.Column<string>(
                        type: "varchar(6)",
                        unicode: false,
                        maxLength: 6,
                        nullable: false
                    ),
                    finishing_spot = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("PK_broadcast_competitor", x => new { x.broadcast_id, x.competing_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_broadcast_competitor_broadcast_half_Enum",
                        "[broadcast_half] IN ('First', 'Second')"
                    );
                    table.CheckConstraint("CK_broadcast_competitor_finishing_spot", "finishing_spot >= 1");
                    table.CheckConstraint("CK_broadcast_competitor_performing_spot", "performing_spot >= 1");
                    table.ForeignKey(
                        name: "FK_broadcast_competitor_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "broadcast_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "broadcast_jury",
                schema: "v0",
                columns: table => new
                {
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_awarded = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("PK_broadcast_jury", x => new { x.broadcast_id, x.voting_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_broadcast_jury_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "broadcast_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "broadcast_televote",
                schema: "v0",
                columns: table => new
                {
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_awarded = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("PK_broadcast_televote", x => new { x.broadcast_id, x.voting_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_broadcast_televote_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "broadcast_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "broadcast_competitor_points_award",
                schema: "v0",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voting_method = table.Column<string>(
                        type: "varchar(8)",
                        unicode: false,
                        maxLength: 8,
                        nullable: false
                    ),
                    points_value = table.Column<int>(type: "int", nullable: false),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("PK_broadcast_competitor_points_award", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_broadcast_competitor_points_award_country_ids",
                        "competing_country_id <> voting_country_id"
                    );
                    table.CheckConstraint(
                        "CK_broadcast_competitor_points_award_points_value",
                        "points_value BETWEEN 0 AND 12"
                    );
                    table.CheckConstraint(
                        "CK_broadcast_competitor_points_award_voting_method_Enum",
                        "[voting_method] IN ('Jury', 'Televote')"
                    );
                    table.ForeignKey(
                        name: "FK_broadcast_competitor_points_award_broadcast_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalSchema: "v0",
                        principalTable: "broadcast_competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_broadcast_competitor_points_award_broadcast_id_competing_country_id_voting_country_id_voting_method",
                schema: "v0",
                table: "broadcast_competitor_points_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id", "voting_method" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_broadcast_competitor_points_award_competing_country_id",
                schema: "v0",
                table: "broadcast_competitor_points_award",
                column: "competing_country_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_broadcast_competitor_points_award_voting_country_id",
                schema: "v0",
                table: "broadcast_competitor_points_award",
                column: "voting_country_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "broadcast_competitor_points_award", schema: "v0");

            migrationBuilder.DropTable(name: "broadcast_jury", schema: "v0");

            migrationBuilder.DropTable(name: "broadcast_televote", schema: "v0");

            migrationBuilder.DropTable(name: "broadcast_competitor", schema: "v0");

            migrationBuilder.DropTable(name: "broadcast", schema: "v0");
        }
    }
}
