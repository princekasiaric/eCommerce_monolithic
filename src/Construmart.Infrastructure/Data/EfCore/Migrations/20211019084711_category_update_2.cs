using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class category_update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsParent",
                table: "Category",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsParent",
                table: "Category");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d791c06d-a96e-4e99-bf96-ad5756a8bfe8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fe918885-a2b8-4a10-9283-79595ab8c5f2", "AQAAAAEAACcQAAAAEHsGOzBasHnA11VZzvPGES/rA9CgDQy2K+Xa0KqC0Cz90NEqxc9+FbBjXIe0wNGeZw==", "e811024e-8cdb-4a54-93d1-3a4bf6a07675" });
        }
    }
}
