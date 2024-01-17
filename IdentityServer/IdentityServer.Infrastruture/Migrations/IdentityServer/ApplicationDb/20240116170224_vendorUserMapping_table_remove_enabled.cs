using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServer.Infrastruture.Migrations.IdentityServer.ApplicationDb
{
    public partial class vendorUserMapping_table_remove_enabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "VendorUserIdMappings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "VendorUserIdMappings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
