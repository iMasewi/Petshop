using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginUpLevel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Userid",
                table: "Products",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_Userid",
                table: "Products",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_Userid",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Userid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "Products");
        }
    }
}
