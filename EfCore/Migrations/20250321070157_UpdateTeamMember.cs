using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeamMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddedById",
                table: "TeamMembers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedOn",
                table: "TeamMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_AddedById",
                table: "TeamMembers",
                column: "AddedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Users_AddedById",
                table: "TeamMembers",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Users_AddedById",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_AddedById",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "TeamMembers");
        }
    }
}
