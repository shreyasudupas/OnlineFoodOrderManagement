using Microsoft.EntityFrameworkCore.Migrations;

namespace BuisnessLayer.Migrations
{
    public partial class update_Citytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCity_tblState_StateId",
                table: "tblCity");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCity_tblState_StateId",
                table: "tblCity",
                column: "StateId",
                principalTable: "tblState",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCity_tblState_StateId",
                table: "tblCity");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCity_tblState_StateId",
                table: "tblCity",
                column: "StateId",
                principalTable: "tblState",
                principalColumn: "StateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
