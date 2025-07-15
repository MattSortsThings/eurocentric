using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Infrastructure.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ModifyContestAggregateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_contest_child_broadcast_broadcast_status_enum",
                table: "contest_child_broadcast");

            migrationBuilder.DropColumn(
                name: "broadcast_status",
                table: "contest_child_broadcast");

            migrationBuilder.AddColumn<bool>(
                name: "completed",
                table: "contest_child_broadcast",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "completed",
                table: "contest_child_broadcast");

            migrationBuilder.AddColumn<int>(
                name: "broadcast_status",
                table: "contest_child_broadcast",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "ck_contest_child_broadcast_broadcast_status_enum",
                table: "contest_child_broadcast",
                sql: "[broadcast_status] IN (0,1,2)");
        }
    }
}
