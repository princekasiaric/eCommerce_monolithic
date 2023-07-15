using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class category_update_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentCategoryId",
                table: "Category",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "fe1e4e7f-ebcd-40c2-8721-645de1b4fd0d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "38cdbc0a-c6c8-4584-b4ba-865bb2f8dc12", "AQAAAAEAACcQAAAAELIzJo8fIunF2sswDXQijFin9iia2q+N4SnLdmHYomzyjAfh1a3NjuG+WKOSH9rUIQ==", "9c4dd086-ba44-43a4-afe7-8a9477cc771f" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "Category");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d353c4ab-b05e-49a8-89c1-9c87d8966607");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "534317a0-cac6-4cd2-815e-bdc2d00af97b", "AQAAAAEAACcQAAAAENikL19sWwofQ5siy/jCKYT0+SrvSnCrxbwSSQS/TZ41fGKEFuSSUj8nVA7l8OpbcA==", "1e9e2d10-a7c9-4c22-996a-937164a95dd1" });
        }
    }
}
