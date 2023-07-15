using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class super_admin_seeder_update1_26_08_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "581682c4-8555-4c9d-a72a-61fded0cae04");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "cf10e685-dac2-4dc4-87d6-36b0a9df8623", "AQAAAAEAACcQAAAAECxmYvz+OWn48kPaq6XmqPq+e/1VWiC2PX09R8JYs6yLC5atJjDyUuVMGsWvN1SzIA==", "20bb86d6-6955-4c12-816c-e2f633a2c0cc", "testadmin@email.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "0dba3e0d-7ae7-42e1-8adf-f11cb82c7f14");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "dada28ea-9181-495b-a45a-5e15598a3e74", "AQAAAAEAACcQAAAAEBQ8bVigiqo8TZx2fpdFi3MHkZ5cMqOz+n2quRCH6lyS946dyv55JsH6u4sAFgeeeQ==", "593378d6-cd5e-42aa-896e-d580618c7499", null });
        }
    }
}
