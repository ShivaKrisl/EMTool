using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfCore.Migrations
{
    /// <inheritdoc />
    public partial class PRUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadyForApproval",
                table: "PullRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadyForApproval",
                table: "PullRequests");
        }
    }
}
