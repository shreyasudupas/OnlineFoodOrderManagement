using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.MicroService.Migrations
{
    public partial class UserAddress_IsActive_ColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserAddress",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserAddress");
        }
    }
}
