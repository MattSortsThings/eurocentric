using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Create_placeholder_country_aggregate_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "placeholder");

            migrationBuilder.CreateTable(
                name: "country",
                schema: "placeholder",
                columns: table => new
                {
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    country_code = table.Column<string>(
                        type: "char(2)",
                        unicode: false,
                        fixedLength: true,
                        maxLength: 2,
                        nullable: false
                    ),
                    country_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    country_type = table.Column<string>(
                        type: "varchar(6)",
                        unicode: false,
                        maxLength: 6,
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.country_id).Annotation("SqlServer:Clustered", true);
                    table.UniqueConstraint("AK_country_country_code", x => x.country_code);
                    table.CheckConstraint("CK_country_country_type_Enum", "[country_type] IN ('Real', 'Pseudo')");
                }
            );

            migrationBuilder.CreateTable(
                name: "country_contest_role",
                schema: "placeholder",
                columns: table => new
                {
                    row_id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    contest_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contest_role_type = table.Column<string>(
                        type: "varchar(14)",
                        unicode: false,
                        maxLength: 14,
                        nullable: false
                    ),
                    country_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country_contest_role", x => x.row_id);
                    table.CheckConstraint(
                        "CK_country_contest_role_contest_role_type_Enum",
                        "[contest_role_type] IN ('Participant', 'GlobalTelevote')"
                    );
                    table.ForeignKey(
                        name: "FK_country_contest_role_country_country_id",
                        column: x => x.country_id,
                        principalSchema: "placeholder",
                        principalTable: "country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_country_contest_role_contest_id_country_id",
                schema: "placeholder",
                table: "country_contest_role",
                columns: new[] { "contest_id", "country_id" },
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_country_contest_role_country_id",
                schema: "placeholder",
                table: "country_contest_role",
                column: "country_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "country_contest_role", schema: "placeholder");

            migrationBuilder.DropTable(name: "country", schema: "placeholder");
        }
    }
}
