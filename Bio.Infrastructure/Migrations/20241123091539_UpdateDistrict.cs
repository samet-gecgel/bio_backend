using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDistrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePathCv",
                table: "Resumes");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "FilePathCv",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
