using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class category_update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image_BaseUrl",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "Image_Path",
                table: "Category",
                newName: "Image_UploadUrl");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image_UploadUrl",
                table: "Category",
                newName: "Image_Path");

            migrationBuilder.AddColumn<string>(
                name: "Image_BaseUrl",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "3f5c8b4b-5012-4519-a8a5-174f6449b5ff");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b9cae94-8388-4578-80e1-ed885ffd1baa", "AQAAAAEAACcQAAAAEKOeZWHCHIK63Z6a6UR5WN0EInreMjZJG9J/vlQ4xNN8eGnfCkzO+rjyozOyQNt1OA==", "607b4286-1b7d-48c9-90cc-0dc837b79fe4" });
        }
    }
}
