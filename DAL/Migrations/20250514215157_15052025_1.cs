using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _15052025_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Plate_plateid",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plate",
                table: "Plate");

            migrationBuilder.RenameTable(
                name: "Plate",
                newName: "Plates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plates",
                table: "Plates",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Plates_plateid",
                table: "OrderItems",
                column: "plateid",
                principalTable: "Plates",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Plates_plateid",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plates",
                table: "Plates");

            migrationBuilder.RenameTable(
                name: "Plates",
                newName: "Plate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plate",
                table: "Plate",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Plate_plateid",
                table: "OrderItems",
                column: "plateid",
                principalTable: "Plate",
                principalColumn: "id");
        }
    }
}
