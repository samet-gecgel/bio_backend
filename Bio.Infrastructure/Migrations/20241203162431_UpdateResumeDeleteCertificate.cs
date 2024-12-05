using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResumeDeleteCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
