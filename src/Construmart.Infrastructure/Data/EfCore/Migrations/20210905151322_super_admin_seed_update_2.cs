using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class super_admin_seed_update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "8a1514bc-7122-40a0-917e-f882ba0ad990", "AQAAAAEAACcQAAAAEJTwQW8HO2j7J3Xo/OV96shwJZ6O7bz1dezBnyoVO0Qk5crOVVJ0VZtQiFyCSNLNGQ==", "4dae2a7e-2875-4b1a-b338-01a287f9e8e2", "testadmin@email.comSuperAdmin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "84070f7b-795a-42b2-8af2-e40793f870dc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "6446c84b-4af7-42ab-8247-e25a460dcc78", "AQAAAAEAACcQAAAAEPySKKQUfETq3zHBUVqVPEvY8qnqhZmk7rmoF83QSTZiMOXbpQoj5Eu1fo8l3uqYBQ==", "160d3602-8268-495d-8fdb-a143691d1db1", "testadmin@email.com" });
        }
    }
}
