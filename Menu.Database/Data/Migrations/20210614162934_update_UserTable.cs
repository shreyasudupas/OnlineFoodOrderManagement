using Microsoft.EntityFrameworkCore.Migrations;

namespace BuisnessLayer.Migrations
{
    public partial class update_UserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tblUser_CityId",
                table: "tblUser",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUser_StateId",
                table: "tblUser",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblUser_tblCity_CityId",
                table: "tblUser",
                column: "CityId",
                principalTable: "tblCity",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblUser_tblState_StateId",
                table: "tblUser",
                column: "StateId",
                principalTable: "tblState",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblUser_tblCity_CityId",
                table: "tblUser");

            migrationBuilder.DropForeignKey(
                name: "FK_tblUser_tblState_StateId",
                table: "tblUser");

            migrationBuilder.DropIndex(
                name: "IX_tblUser_CityId",
                table: "tblUser");

            migrationBuilder.DropIndex(
                name: "IX_tblUser_StateId",
                table: "tblUser");
        }
    }
}
