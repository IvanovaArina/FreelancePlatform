using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancePlatformConnect.Api.Migrations
{
    /// <inheritdoc />
    public partial class Newpotfolioitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "PortfolioItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ImageStatus",
                table: "PortfolioItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TempImageKey",
                table: "PortfolioItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageStatus",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "TempImageKey",
                table: "PortfolioItems");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "PortfolioItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
