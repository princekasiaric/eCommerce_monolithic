using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class addedProductIsActiveState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "5ed5d81c-6513-4295-8b58-ef8695593b7d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80a76251-f8b3-4c03-8fc8-14e198e5e6c9", "AQAAAAEAACcQAAAAEAEkkHB+iEITrghQRbruoauCHLAUca4kQ+a7QGqkb8WbJI2+jDVb2nt2AV85xaPxgg==", "0c89ea52-d07f-49c0-bb16-d9da9bf78841" });
        }
    }
}
