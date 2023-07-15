using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class super_admin_seed_update_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "077bf92b-1df7-4393-95bf-414d90303a44", "TESTADMIN@EMAIL.COMSUPERADMIN", "AQAAAAEAACcQAAAAED0IiY0EZpqNfbKJCJp7omvIlCgaf/Kb+cjvYcQ7f563HkjVPg8qOxuiRcZRoVbxJQ==", "f0823ad2-0ac0-40d3-ad1c-9b731e9d0ce1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "1488d48b-00fd-4228-937a-8229a9ba483c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a1514bc-7122-40a0-917e-f882ba0ad990", null, "AQAAAAEAACcQAAAAEJTwQW8HO2j7J3Xo/OV96shwJZ6O7bz1dezBnyoVO0Qk5crOVVJ0VZtQiFyCSNLNGQ==", "4dae2a7e-2875-4b1a-b338-01a287f9e8e2" });
        }
    }
}
