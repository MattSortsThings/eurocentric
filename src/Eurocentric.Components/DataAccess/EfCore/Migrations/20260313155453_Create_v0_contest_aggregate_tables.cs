using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Create_v0_contest_aggregate_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contest",
                schema: "v0",
                columns: table => new
                {
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_year = table.Column<int>(type: "int", nullable: false),
                    city_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    semi_final_broadcast_format = table.Column<string>(
                        type: "varchar(15)",
                        unicode: false,
                        maxLength: 15,
                        nullable: false
                    ),
                    grand_final_broadcast_format = table.Column<string>(
                        type: "varchar(15)",
                        unicode: false,
                        maxLength: 15,
                        nullable: false
                    ),
                    queryable = table.Column<bool>(type: "bit", nullable: false),
                    global_televote_voting_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contest", x => x.contest_id).Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("AK_contest_contest_year", x => x.contest_year);
                    table.CheckConstraint("CK_contest_contest_year", "contest_year BETWEEN 2016 AND 2030");
                    table.CheckConstraint(
                        "CK_contest_grand_final_broadcast_format_Enum",
                        "[grand_final_broadcast_format] IN ('JuryAndTelevote', 'TelevoteOnly')"
                    );
                    table.CheckConstraint(
                        "CK_contest_semi_final_broadcast_format_Enum",
                        "[semi_final_broadcast_format] IN ('JuryAndTelevote', 'TelevoteOnly')"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "contest_child_broadcast",
                schema: "v0",
                columns: table => new
                {
                    child_broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<string>(
                        type: "varchar(10)",
                        unicode: false,
                        maxLength: 10,
                        nullable: false
                    ),
                    completed = table.Column<bool>(type: "bit", nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("PK_contest_child_broadcast", x => new { x.contest_id, x.child_broadcast_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_contest_child_broadcast_contest_stage_Enum",
                        "[contest_stage] IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')"
                    );
                    table.ForeignKey(
                        name: "FK_contest_child_broadcast_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "contest_participant",
                schema: "v0",
                columns: table => new
                {
                    participating_country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    semi_final_draw = table.Column<string>(
                        type: "varchar(10)",
                        unicode: false,
                        maxLength: 10,
                        nullable: false
                    ),
                    act_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    song_title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("PK_contest_participant", x => new { x.contest_id, x.participating_country_id })
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_contest_participant_semi_final_draw_Enum",
                        "[semi_final_draw] IN ('SemiFinal1', 'SemiFinal2')"
                    );
                    table.ForeignKey(
                        name: "FK_contest_participant_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "v0",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_contest_child_broadcast_contest_id_contest_stage",
                schema: "v0",
                table: "contest_child_broadcast",
                columns: new[] { "contest_id", "contest_stage" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "contest_child_broadcast", schema: "v0");

            migrationBuilder.DropTable(name: "contest_participant", schema: "v0");

            migrationBuilder.DropTable(name: "contest", schema: "v0");
        }
    }
}
