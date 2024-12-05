using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Resumes_ResumeId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_ResumeId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "ResumeId",
                table: "Certificates");

            migrationBuilder.AddColumn<Guid>(
                name: "CertificateId",
                table: "Resumes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_CertificateId",
                table: "Resumes",
                column: "CertificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resumes_Certificates_CertificateId",
                table: "Resumes",
                column: "CertificateId",
                principalTable: "Certificates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resumes_Certificates_CertificateId",
                table: "Resumes");

            migrationBuilder.DropIndex(
                name: "IX_Resumes_CertificateId",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                table: "Resumes");

            migrationBuilder.AddColumn<Guid>(
                name: "ResumeId",
                table: "Certificates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ResumeId",
                table: "Certificates",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Resumes_ResumeId",
                table: "Certificates",
                column: "ResumeId",
                principalTable: "Resumes",
                principalColumn: "Id");
        }
    }
}
