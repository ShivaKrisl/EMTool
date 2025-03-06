using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ScrumMeetingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ScrumMeetings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByRole",
                table: "ScrumMeetings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InvitedUsersIds",
                table: "ScrumMeetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateIndex(
                name: "IX_ScrumMeetings_CreatedBy",
                table: "ScrumMeetings",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ScrumMeetings_Users_CreatedBy",
                table: "ScrumMeetings",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScrumMeetings_Users_CreatedBy",
                table: "ScrumMeetings");

            migrationBuilder.DropIndex(
                name: "IX_ScrumMeetings_CreatedBy",
                table: "ScrumMeetings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ScrumMeetings");

            migrationBuilder.DropColumn(
                name: "CreatedByRole",
                table: "ScrumMeetings");

            migrationBuilder.DropColumn(
                name: "InvitedUsersIds",
                table: "ScrumMeetings");
        }
    }
}
