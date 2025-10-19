﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eurocentric.Components.DataAccess.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class Add_v0_usp_get_competing_country_points_average_rankings : Migration
    {
        private const string UpScript = "20251019134028_Add_v0_usp_get_competing_country_points_average_rankings.sql";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.ExecuteSqlFromScript(UpScript);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE v0.usp_get_competing_country_points_average_rankings;");
        }
    }
}
