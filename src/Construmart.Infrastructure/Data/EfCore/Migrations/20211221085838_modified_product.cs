using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class modified_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage");

            migrationBuilder.AddColumn<long>(
                name: "ProductImageId",
                table: "Product",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "c2eeb011-9aa5-47cc-9a26-543fa8336a1a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da9b1d06-70e3-4f61-9a67-8238b45e7d2d", "AQAAAAEAACcQAAAAEFGxBrbizhmioxO/Jo9BBM8IHxEVhrIbHbU6WmTmR6vu1bWUGye59MVfDjyo7BipzQ==", "31befab7-3eb5-4086-ba20-889e86d2e6a8" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "ProductImageId",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "56cfa1e7-8a07-472a-bf02-ff1bd7977f53");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a7904a9d-71b7-4425-94f4-e418ed21e466", "AQAAAAEAACcQAAAAEEX1RPcqzS3zhvGXAvqeVI76r4lIstwMBMlzYdgB2D1uzinEVo4ebJQ61DJZM0U6rw==", "80175162-f0b8-473f-ab48-0867452bbbd7" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");
        }
    }
}
