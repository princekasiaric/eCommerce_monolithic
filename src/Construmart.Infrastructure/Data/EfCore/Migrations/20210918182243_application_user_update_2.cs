using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class application_user_update_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "fa803422-f90d-483d-830c-392a6a1e877e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "c017689e-ce8b-4d76-8f99-ce1661a797b4", "TESTADMIN@EMAIL.COMADMINISTRATORAPP", "AQAAAAEAACcQAAAAEDGhW7HVtKaIHFiAwZE5yRBA96PrX8cGqRNuxh2uFVr89Ps9mU1jHUuhZod3JU6gmw==", "8fb883cb-cfe2-499c-9107-2c404ea81c39", "testadmin@email.comAdministratorApp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "bfd8e655-eadf-4948-af6d-994b4ce35a34");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "7ec2b9bb-d291-449f-a6e6-3340487ed245", "TESTADMIN@EMAIL.COMSUPERADMIN", "AQAAAAEAACcQAAAAEEzKD87+qjV8DLM8gr6x9YTVIti3VAq0i3xpRGb5OKOoYOp2LqZyJR5cWFBflFqcww==", "c750c846-2281-47a7-be24-eb579c6c33aa", "testadmin@email.comSuperAdmin" });
        }
    }
}
