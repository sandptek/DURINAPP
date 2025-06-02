using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _15052025_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(type: "int", nullable: false),
                    orderItemid = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    dueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    completedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    createdUserID = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    updatedUserID = table.Column<int>(type: "int", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tasks_OrderItems_orderItemid",
                        column: x => x.orderItemid,
                        principalTable: "OrderItems",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Tasks_Users_userid",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taskid = table.Column<int>(type: "int", nullable: false),
                    userid = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdUserID = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    updatedUserID = table.Column<int>(type: "int", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.id);
                    table.ForeignKey(
                        name: "FK_TaskComments_Tasks_taskid",
                        column: x => x.taskid,
                        principalTable: "Tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_taskid",
                table: "TaskComments",
                column: "taskid");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_userid",
                table: "TaskComments",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_orderItemid",
                table: "Tasks",
                column: "orderItemid");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_userid",
                table: "Tasks",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
