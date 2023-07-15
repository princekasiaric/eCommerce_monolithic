using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class modified_category_model : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "ImageFile_SecureUploadUrl",
                table: "ProductImage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "e90cc6bd-d430-42f1-b6be-b0d1e5f191da");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7654341e-928c-42cf-896c-fc5d495cbad0", "AQAAAAEAACcQAAAAEO0XxMqnP9mzweDBsSR8FdSYk2qhcTT0vPbC4yx2RCObGk0K5K7speNeFNV4t0aWew==", "b47aad2f-d058-4913-b303-d41ad0d0a490" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFile_SecureUploadUrl",
                table: "ProductImage");

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
                value: "5ed5d81c-6513-4295-8b58-ef8695593b7d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80a76251-f8b3-4c03-8fc8-14e198e5e6c9", "AQAAAAEAACcQAAAAEAEkkHB+iEITrghQRbruoauCHLAUca4kQ+a7QGqkb8WbJI2+jDVb2nt2AV85xaPxgg==", "0c89ea52-d07f-49c0-bb16-d9da9bf78841" });
        }
    }
}
