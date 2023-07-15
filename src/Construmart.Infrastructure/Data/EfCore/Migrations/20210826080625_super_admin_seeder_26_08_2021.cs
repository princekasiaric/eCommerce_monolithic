using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class super_admin_seeder_26_08_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dada28ea-9181-495b-a45a-5e15598a3e74", "AQAAAAEAACcQAAAAEBQ8bVigiqo8TZx2fpdFi3MHkZ5cMqOz+n2quRCH6lyS946dyv55JsH6u4sAFgeeeQ==", "593378d6-cd5e-42aa-896e-d580618c7499" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "9330cedc-79b9-4234-b03f-686d47f1056f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b4d72c71-c41d-4f22-8639-533aa30d2457", "AQAAAAEAACcQAAAAENME/PfscltDvG8NWGNhf6C5HZDW0gzaPa4LRaV2SYNlV8eNY2ZfyQrAKiP621VWOw==", null });
        }
    }
}
