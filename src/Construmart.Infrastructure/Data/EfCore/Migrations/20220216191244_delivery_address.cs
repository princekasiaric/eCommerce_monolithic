using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class delivery_address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPriceCurrencyCode",
                table: "Product",
                newName: "CurrencyCode");

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LGA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NigerianStateId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<string>(type: "nvarchar(max)", rowVersion: true, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedByUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "edeec0ea-d4c3-4dfa-b717-52b3433777cc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e7d34db-d878-4c73-9744-40e98ff5f518", "AQAAAAEAACcQAAAAEEF1evKIRWiNNj8s0rHb9YM6ekFyyuTgv1xd63YALzaWdS1y3FcikJY7tl7H7KNWCw==", "bc77083f-8188-4828-a0b7-3d4fc1254b50" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Product",
                newName: "UnitPriceCurrencyCode");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2d53660a-e427-41d9-aa81-7dc7d5fb4e72");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e637971-1849-4d1e-bacd-9433a817d425", "AQAAAAEAACcQAAAAECs7ZVXwYCqnhazlT7oJ7ZL8X0DakNmxCDIrquvGEzV98CKahN6oBQrtSYzF4HaqXQ==", "16616986-dab1-4796-8d3d-13db89e2e3e1" });
        }
    }
}
