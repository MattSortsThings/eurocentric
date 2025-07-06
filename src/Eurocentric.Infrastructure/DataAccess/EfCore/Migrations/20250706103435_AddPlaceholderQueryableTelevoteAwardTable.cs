using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaceholderQueryableTelevoteAwardTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "placeholder_queryable_televote_award",
                columns: table => new
                {
                    contest_year = table.Column<int>(type: "int", nullable: false),
                    contest_stage = table.Column<int>(type: "int", nullable: false),
                    competing_country_code = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    voting_country_code = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    broadcast_tag = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    running_order_position = table.Column<int>(type: "int", nullable: false),
                    points_value = table.Column<int>(type: "int", nullable: false),
                    max_points_value = table.Column<int>(type: "int", nullable: false),
                    real_points_value = table.Column<double>(type: "float", nullable: false),
                    normalized_points_value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_placeholder_queryable_televote_award", x => new { x.competing_country_code, x.voting_country_code, x.contest_year, x.contest_stage });
                    table.CheckConstraint("ck_placeholder_queryable_televote_award_contest_stage_enum", "[contest_stage] IN (0,1,2)");
                });

            migrationBuilder.CreateIndex(
                name: "ix_placeholder_queryable_televote_award_competing_country_code",
                table: "placeholder_queryable_televote_award",
                column: "competing_country_code");

            migrationBuilder.CreateIndex(
                name: "ix_placeholder_queryable_televote_award_contest_stage",
                table: "placeholder_queryable_televote_award",
                column: "contest_stage");

            migrationBuilder.CreateIndex(
                name: "ix_placeholder_queryable_televote_award_contest_year",
                table: "placeholder_queryable_televote_award",
                column: "contest_year");

            migrationBuilder.CreateIndex(
                name: "ix_placeholder_queryable_televote_award_voting_country_code",
                table: "placeholder_queryable_televote_award",
                column: "voting_country_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "placeholder_queryable_televote_award");
        }
    }
}
