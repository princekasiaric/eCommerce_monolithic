using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class modified_product_inventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityRemoved",
                table: "ProductInventory");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityRemoved",
                table: "ProductInventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "209e0274-bdde-41bb-ac2b-be13b3d2f5cd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c849906-64fd-443a-8b9d-2366a4a98caf", "AQAAAAEAACcQAAAAEPy2x3HJ3irQtCVwHbYZwSL00CcEFykX/GpjqqpRu9j7s7INMSxzFQ1GpuUvbzHV8Q==", "5469c1e0-c5c1-4242-9d0a-3cd9e5817177" });
        }
    }
}
