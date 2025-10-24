using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_dbo_country_aggregate_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "dbo");

            migrationBuilder.CreateTable(
                name: "country",
                schema: "dbo",
                columns: table => new
                {
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_code = table.Column<string>(
                        type: "nchar(2)",
                        fixedLength: true,
                        maxLength: 2,
                        nullable: false
                    ),
                    country_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country", x => x.country_id).Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("ak_country_country_code", x => x.country_code);
                }
            );

            migrationBuilder.CreateTable(
                name: "country_contest_role",
                schema: "dbo",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_role_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country_contest_role", x => x.row_id).Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint(
                        "CK_country_contest_role_contest_role_type_Enum",
                        "[contest_role_type] IN (N'Participant', N'GlobalTelevote')"
                    );
                    table.ForeignKey(
                        name: "fk_country_contest_role_country_country_id",
                        column: x => x.country_id,
                        principalSchema: "dbo",
                        principalTable: "country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "ix_country_contest_role_country_id_contest_id",
                schema: "dbo",
                table: "country_contest_role",
                columns: new[] { "country_id", "contest_id" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "country_contest_role", schema: "dbo");

            migrationBuilder.DropTable(name: "country", schema: "dbo");
        }
    }
}
