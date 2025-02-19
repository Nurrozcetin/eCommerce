using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commerce.Migrations
{
    /// <inheritdoc />
    public partial class OrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusID",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_StatusID",
                table: "OrderItem",
                column: "StatusID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Status_StatusID",
                table: "OrderItem",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "StatusID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Status_StatusID",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_StatusID",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "StatusID",
                table: "OrderItem");
        }
    }
}
