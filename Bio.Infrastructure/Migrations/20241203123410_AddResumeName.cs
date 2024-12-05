using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeName",
                table: "Resumes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeName",
                table: "Resumes");
        }
    }
}
