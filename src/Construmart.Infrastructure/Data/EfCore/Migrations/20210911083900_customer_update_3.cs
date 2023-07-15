using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class customer_update_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_StreetName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_StreetNumber",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "00bdd65f-9f70-449c-bbc6-43dd67e51d2e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7c1b8060-b74b-4e0e-a960-367f2971ee37", "AQAAAAEAACcQAAAAEPSV6x7NOXwuSHFJ6F/YBcIdazC9W2d8mi+daTpYw8YVaMPoKkzsZx51S3+Gdw6QRg==", "0236e365-0968-4c8d-9dd7-0bdff219482e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Address_StreetName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Address_StreetNumber",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "Customer");

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
    }
}
