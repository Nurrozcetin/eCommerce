using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Commerce.Migrations
{
    /// <inheritdoc />
    public partial class Question : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Question");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Question",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Question_ProductID",
                table: "Question",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Product_ProductID",
                table: "Question",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Product_ProductID",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_ProductID",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Question");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Question",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
