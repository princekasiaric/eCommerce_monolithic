using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class model_state_sync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image_Extension",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Image_Name",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Image_UploadUrl",
                table: "Category");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "0de4869c-25e2-4f92-adf4-d1a14ad8bbba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da5ab3a6-9292-450b-9e23-a06ac9022c3d", "AQAAAAEAACcQAAAAECHrMyVlJUxDHLV4J1W6mxBpad5KoBwaa5QJSRDWA0u5Vqlvlwb5AHN+lunVCXnGJw==", "20560e8e-e72f-4c5f-b1ca-cda77a44b01e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image_Extension",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image_Name",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image_UploadUrl",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "c2eeb011-9aa5-47cc-9a26-543fa8336a1a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da9b1d06-70e3-4f61-9a67-8238b45e7d2d", "AQAAAAEAACcQAAAAEFGxBrbizhmioxO/Jo9BBM8IHxEVhrIbHbU6WmTmR6vu1bWUGye59MVfDjyo7BipzQ==", "31befab7-3eb5-4086-ba20-889e86d2e6a8" });
        }
    }
}