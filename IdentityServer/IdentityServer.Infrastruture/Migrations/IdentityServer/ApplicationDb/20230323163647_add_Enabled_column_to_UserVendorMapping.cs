using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServer.Infrastruture.Migrations.IdentityServer.ApplicationDb
{
    public partial class add_Enabled_column_to_UserVendorMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "VendorUserIdMappings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "VendorUserIdMappings");
        }
    }
}
