using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class OrderAndCartModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "OrderItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "d8bdea7f-fc17-4026-b5f3-2bb6d6d774e2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95f1dcad-fda1-4b08-8b26-7fff714e559b", "AQAAAAEAACcQAAAAEHSYPT3gB5E3oEYovUcuZytKDQbWJ8Wh7VpkIw5pTFaXZKyKs29EehyNELPG9p/Ovg==", "e7b932ff-8466-4a5d-acf0-ecf701742a5a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "OrderItem",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "c1452ac7-6748-43a1-972f-2f84ab3dd6e8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "be95e425-3aa0-458a-bbe7-1010dc8b5ddf", "AQAAAAEAACcQAAAAEEM5KWfrdxEoPZ745NEVnJP64P6DIOuFJ6hCMQO9OCRBdAf8HYhVODs49rzf7uFFiA==", "a80813a7-8141-47bd-98b5-617651c1b003" });
        }
    }
}
