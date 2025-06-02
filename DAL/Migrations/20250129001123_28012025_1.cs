using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _28012025_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CCL19",
                table: "OrderItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DNAJC8",
                table: "OrderItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IL20",
                table: "OrderItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "KAPPA",
                table: "OrderItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LGALS1",
                table: "OrderItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MARK1",
                table: "OrderItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MGC31944",
                table: "OrderItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PUSL1",
                table: "OrderItems",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCL19",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DNAJC8",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IL20",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "KAPPA",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "LGALS1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "MARK1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "MGC31944",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PUSL1",
                table: "OrderItems");
        }
    }
}
