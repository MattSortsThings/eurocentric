using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
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
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    broadcast_date = table.Column<DateOnly>(type: "date", nullable: false),
                    parent_contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast", x => x.broadcast_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_broadcast_broadcast_date", x => x.broadcast_date);
                    table.UniqueConstraint("ak_broadcast_parent_contest_id_contest_stage", x => new { x.parent_contest_id, x.contest_stage });
                    table.CheckConstraint("ck_broadcast_broadcast_date", "broadcast_date BETWEEN '2016-01-01' AND '2050-12-31'");
                    table.CheckConstraint("CK_broadcast_contest_stage_Enum", "[contest_stage] IN (N'SemiFinal1', N'SemiFinal2', N'GrandFinal')");
                });

            migrationBuilder.CreateTable(
                name: "contest",
                schema: "v0",
                columns: table => new
                {
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_year = table.Column<int>(type: "int", nullable: false),
                    city_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    contest_rules = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    queryable = table.Column<bool>(type: "bit", nullable: false),
                    global_televote_voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest", x => x.contest_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_contest_contest_year", x => x.contest_year);
                    table.CheckConstraint("CK_contest_contest_rules_Enum", "[contest_rules] IN (N'Liverpool', N'Stockholm')");
                    table.CheckConstraint("ck_contest_contest_year", "contest_year BETWEEN 2016 AND 2050");
                    table.CheckConstraint("ck_contest_global_televote_nullability", "(contest_rules = N'Liverpool' AND global_televote_voting_country_id IS NOT NULL) OR (contest_rules = N'Stockholm' AND global_televote_voting_country_id IS NULL)");
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
                name: "broadcast_competitor",
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
                    table.PrimaryKey("ak_broadcast_competitor_broadcast_id_competing_country_id", x => new { x.broadcast_id, x.competing_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_broadcast_competitor_finishing_position", "finishing_position >= 1");
                    table.CheckConstraint("ck_broadcast_competitor_running_order_spot", "running_order_spot >= 1");
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_broadcast_broadcast_id",
                        column: x => x.broadcast_id,
                        principalSchema: "v0",
                        principalTable: "broadcast",
                        principalColumn: "broadcast_id",
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
                        principalColumn: "broadcast_id",
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
                        principalColumn: "broadcast_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_child_broadcast",
                schema: "v0",
                columns: table => new
                {
                    child_broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest_child_broadcast", x => new { x.contest_id, x.child_broadcast_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("CK_contest_child_broadcast_contest_stage_Enum", "[contest_stage] IN (N'SemiFinal1', N'SemiFinal2', N'GrandFinal')");
                    table.ForeignKey(
                        name: "fk_contest_child_broadcast_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_participant",
                schema: "v0",
                columns: table => new
                {
                    participating_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    semi_final_draw = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    act_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    song_title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest_participant", x => new { x.contest_id, x.participating_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("CK_contest_participant_semi_final_draw_Enum", "[semi_final_draw] IN (N'SemiFinal1', N'SemiFinal2')");
                    table.ForeignKey(
                        name: "fk_contest_participant_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "country_contest_role",
                schema: "v0",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_role_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country_contest_role", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("CK_country_contest_role_contest_role_type_Enum", "[contest_role_type] IN (N'Participant', N'GlobalTelevote')");
                    table.ForeignKey(
                        name: "fk_country_contest_role_country_country_id",
                        column: x => x.country_id,
                        principalSchema: "v0",
                        principalTable: "country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "broadcast_competitor_jury_award",
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
                    table.PrimaryKey("pk_broadcast_competitor_jury_award", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_broadcast_competitor_jury_award_country_ids", "competing_country_id <> voting_country_id");
                    table.CheckConstraint("CK_broadcast_competitor_jury_award_points_value_Enum", "[points_value] IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12)");
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
                    row_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    competing_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_broadcast_competitor_televote_award", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("ck_broadcast_competitor_televote_award_country_ids", "competing_country_id <> voting_country_id");
                    table.CheckConstraint("CK_broadcast_competitor_televote_award_points_value_Enum", "[points_value] IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 10, 12)");
                    table.ForeignKey(
                        name: "fk_broadcast_competitor_televote_award_broadcast_competitor_broadcast_id_competing_country_id",
                        columns: x => new { x.broadcast_id, x.competing_country_id },
                        principalSchema: "v0",
                        principalTable: "broadcast_competitor",
                        principalColumns: new[] { "broadcast_id", "competing_country_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_broadcast_id_running_order_spot",
                schema: "v0",
                table: "broadcast_competitor",
                columns: new[] { "broadcast_id", "running_order_spot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_jury_award_broadcast_id_competing_country_id_voting_country_id",
                schema: "v0",
                table: "broadcast_competitor_jury_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_broadcast_competitor_televote_award_broadcast_id_competing_country_id_voting_country_id",
                schema: "v0",
                table: "broadcast_competitor_televote_award",
                columns: new[] { "broadcast_id", "competing_country_id", "voting_country_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contest_child_broadcast_contest_id_contest_stage",
                schema: "v0",
                table: "contest_child_broadcast",
                columns: new[] { "contest_id", "contest_stage" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_country_contest_role_country_id_contest_id",
                schema: "v0",
                table: "country_contest_role",
                columns: new[] { "country_id", "contest_id" },
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
                name: "country_contest_role",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast_competitor",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "contest",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "country",
                schema: "v0");

            migrationBuilder.DropTable(
                name: "broadcast",
                schema: "v0");
        }
    }
}
