using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServer.Infrastruture.Migrations.IdentityServer.ApplicationDb
{
    public partial class Add_UserPointsEvent_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPointsEvents",
                columns: table => new
                {
                    EventId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PointsInHand = table.Column<double>(type: "float", nullable: false),
                    PointsAdjusted = table.Column<double>(type: "float", nullable: false),
                    EventOperation = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    AddOrAdjustedUserId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    EventCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPointsEvents", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPointsEvents");
        }
    }
}
