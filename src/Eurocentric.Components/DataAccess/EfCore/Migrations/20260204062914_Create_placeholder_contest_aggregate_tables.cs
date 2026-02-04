using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Create_placeholder_contest_aggregate_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contest",
                schema: "placeholder",
                columns: table => new
                {
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_year = table.Column<int>(type: "int", nullable: false),
                    city_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    semi_final_voting_format = table.Column<string>(
                        type: "varchar(15)",
                        unicode: false,
                        maxLength: 15,
                        nullable: false
                    ),
                    grand_final_voting_format = table.Column<string>(
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
                    table.CheckConstraint("CK_contest_contest_year", "[contest_year] BETWEEN 2016 AND 2050");
                    table.CheckConstraint(
                        "CK_contest_grand_final_voting_format_Enum",
                        "[grand_final_voting_format] IN ('TelevoteAndJury', 'TelevoteOnly')"
                    );
                    table.CheckConstraint(
                        "CK_contest_semi_final_voting_format_Enum",
                        "[semi_final_voting_format] IN ('TelevoteAndJury', 'TelevoteOnly')"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "contest_broadcast_memo",
                schema: "placeholder",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    broadcast_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_stage = table.Column<string>(
                        type: "varchar(10)",
                        unicode: false,
                        maxLength: 10,
                        nullable: false
                    ),
                    completed = table.Column<bool>(type: "bit", nullable: false),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table
                        .PrimaryKey("PK_contest_broadcast_memo", x => x.row_id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_contest_broadcast_memo_contest_stage_Enum",
                        "[contest_stage] IN ('SemiFinal1', 'SemiFinal2', 'GrandFinal')"
                    );
                    table.ForeignKey(
                        name: "FK_contest_broadcast_memo_contest_contest_id",
                        column: x => x.contest_id,
                        principalSchema: "placeholder",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "contest_participant",
                schema: "placeholder",
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
                        principalSchema: "placeholder",
                        principalTable: "contest",
                        principalColumn: "contest_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_contest_broadcast_memo_contest_id_broadcast_id",
                schema: "placeholder",
                table: "contest_broadcast_memo",
                columns: new[] { "contest_id", "broadcast_id" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "contest_broadcast_memo", schema: "placeholder");

            migrationBuilder.DropTable(name: "contest_participant", schema: "placeholder");

            migrationBuilder.DropTable(name: "contest", schema: "placeholder");
        }
    }
}
