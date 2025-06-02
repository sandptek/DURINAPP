using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _09022025_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mlResponse",
                table: "OrderItems",
                newName: "mlScore2");

            migrationBuilder.AddColumn<string>(
                name: "mlLastError",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mlPrediction",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "mlScore1",
                table: "OrderItems",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mlLastError",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "mlPrediction",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "mlScore1",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "mlScore2",
                table: "OrderItems",
                newName: "mlResponse");
        }
    }
}
