using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class customer_update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OnboardingStatus",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "ce60111c-ce11-46a2-8f26-17b8641424cb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "484cf17c-d31c-4691-a90a-96b2a3a7f49b", "AQAAAAEAACcQAAAAEM0UXlYYWXPNwCd9VFb4FBjCgXhGawdasYhmj8tkXEDLwlsnH5AgMKlYLFwn5Oll1Q==", "ce3e80b9-78b4-4b7a-811a-274404db6bef" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnboardingStatus",
                table: "Customer");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "317224e0-e1d6-4e85-b6d7-b2a819c2d8f4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "077bf92b-1df7-4393-95bf-414d90303a44", "AQAAAAEAACcQAAAAED0IiY0EZpqNfbKJCJp7omvIlCgaf/Kb+cjvYcQ7f563HkjVPg8qOxuiRcZRoVbxJQ==", "f0823ad2-0ac0-40d3-ad1c-9b731e9d0ce1" });
        }
    }
}
