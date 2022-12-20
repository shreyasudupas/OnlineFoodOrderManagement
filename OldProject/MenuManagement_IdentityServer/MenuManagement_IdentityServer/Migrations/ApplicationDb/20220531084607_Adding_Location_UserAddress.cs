using Microsoft.EntityFrameworkCore.Migrations;

namespace MenuManagement_IdentityServer.Migrations.ApplicationDb
{
    public partial class Adding_Location_UserAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "UserAddresses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "UserAddresses");
        }
    }
}
