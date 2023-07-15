using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class application_user_update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Otp_Expiry",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Otp_Hash",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Otp_IsUsed",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Otp_Purpose",
                table: "Customer");

            migrationBuilder.AddColumn<DateTime>(
                name: "Otp_Expiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Otp_Hash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Otp_IsUsed",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Otp_Purpose",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7ec2b9bb-d291-449f-a6e6-3340487ed245", "AQAAAAEAACcQAAAAEEzKD87+qjV8DLM8gr6x9YTVIti3VAq0i3xpRGb5OKOoYOp2LqZyJR5cWFBflFqcww==", "c750c846-2281-47a7-be24-eb579c6c33aa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Otp_Expiry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Otp_Hash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Otp_IsUsed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Otp_Purpose",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "Otp_Expiry",
                table: "Customer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Otp_Hash",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Otp_IsUsed",
                table: "Customer",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Otp_Purpose",
                table: "Customer",
                type: "int",
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
    }
}
