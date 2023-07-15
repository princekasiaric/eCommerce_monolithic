using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class application_user_update_26_08_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b4d72c71-c41d-4f22-8639-533aa30d2457", "AQAAAAEAACcQAAAAENME/PfscltDvG8NWGNhf6C5HZDW0gzaPa4LRaV2SYNlV8eNY2ZfyQrAKiP621VWOw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "ca1b9337-1664-4d7b-bb4e-1c177f99568f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "46df9e00-1cd3-464e-87e0-c42e3c7a37e1", "AQAAAAEAACcQAAAAELNx9CA04giBaqIKO8LL5LsSm+sUtelIidSVzXR/g2ecxYwtd9b2OwdAI+lL4wdrWA==" });
        }
    }
}
