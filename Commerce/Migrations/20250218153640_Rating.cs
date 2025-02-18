using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commerce.Migrations
{
    /// <inheritdoc />
    public partial class Rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Size_SizeID",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_SizeID",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "SizeID",
                table: "Rating");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Rating",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Rating",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Rating",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ProductID",
                table: "Rating",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Product_ProductID",
                table: "Rating",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Product_ProductID",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_ProductID",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Rating");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Rating",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeID",
                table: "Rating",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_SizeID",
                table: "Rating",
                column: "SizeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Size_SizeID",
                table: "Rating",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "SizeID");
        }
    }
}
