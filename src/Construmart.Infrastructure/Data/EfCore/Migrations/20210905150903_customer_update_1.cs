using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class customer_update_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ApplicationUserId",
                table: "Customer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "84070f7b-795a-42b2-8af2-e40793f870dc", "SUPERADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6446c84b-4af7-42ab-8247-e25a460dcc78", "AQAAAAEAACcQAAAAEPySKKQUfETq3zHBUVqVPEvY8qnqhZmk7rmoF83QSTZiMOXbpQoj5Eu1fo8l3uqYBQ==", "160d3602-8268-495d-8fdb-a143691d1db1" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Customer",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "bb77313d-409e-44cf-b97f-07d24374d1b4", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b831c342-b6b7-4681-bf2d-0e4f83ddf302", "AQAAAAEAACcQAAAAEP0h2DIudPBEdWw2PnDM31yvWGD9xOBTtr5Io23kgemK0bGfmnbdf0O5bXoWd08eVA==", "a2b661a6-3b8c-4b19-8b92-e6d59bd81167" });
        }
    }
}
