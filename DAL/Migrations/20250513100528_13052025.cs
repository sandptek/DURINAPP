using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _13052025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "plateid",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Plate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdUserID = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedUserID = table.Column<int>(type: "int", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plate", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_plateid",
                table: "OrderItems",
                column: "plateid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Plate_plateid",
                table: "OrderItems",
                column: "plateid",
                principalTable: "Plate",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Plate_plateid",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "Plate");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_plateid",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "plateid",
                table: "OrderItems");
        }
    }
}
