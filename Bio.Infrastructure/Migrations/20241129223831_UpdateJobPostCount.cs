using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobPostCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "JobPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "JobPosts");
        }
    }
}
