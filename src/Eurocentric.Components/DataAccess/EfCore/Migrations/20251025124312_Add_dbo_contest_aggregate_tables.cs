using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_contest_aggregate_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contest",
                schema: "dbo",
                columns: table => new
                {
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_year = table.Column<int>(type: "int", nullable: false),
                    city_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    contest_rules = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    queryable = table.Column<bool>(type: "bit", nullable: false),
                    global_televote_voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contest", x => x.contest_id).Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_contest_contest_year", x => x.contest_year);
                    table.CheckConstraint(
                        "CK_contest_contest_rules_Enum",
                        "[contest_rules] IN (N'Liverpool', N'Stockholm')"
                    );
                    table.CheckConstraint("ck_contest_contest_year", "contest_year BETWEEN 2016 AND 2050");
                }
            );

            migrationBuilder.CreateTable(
                name: "contest_child_broadcast",
                schema: "dbo",
                columns: table => new
                {
                    child_broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    completed = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("pk_contest_child_broadcast", x => new { x.contest_id, x.child_broadcast_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_contest_child_broadcast_contest_stage_Enum",
                        "[contest_stage] IN (N'SemiFinal1', N'SemiFinal2', N'GrandFinal')"
                    );
                    table.ForeignKey(
                        name: "fk_contest_child_broadcast_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "dbo",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "contest_participant",
                schema: "dbo",
                columns: table => new
                {
                    participating_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    semi_final_draw = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    act_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    song_title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("pk_contest_participant", x => new { x.contest_id, x.participating_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_contest_participant_semi_final_draw_Enum",
                        "[semi_final_draw] IN (N'SemiFinal1', N'SemiFinal2')"
                    );
                    table.ForeignKey(
                        name: "fk_contest_participant_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "dbo",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_contest_child_broadcast_contest_id_contest_stage",
                schema: "dbo",
                table: "contest_child_broadcast",
                columns: new[] { "contest_id", "contest_stage" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "contest_child_broadcast", schema: "dbo");

            migrationBuilder.DropTable(name: "contest_participant", schema: "dbo");

            migrationBuilder.DropTable(name: "contest", schema: "dbo");
        }
    }
}
