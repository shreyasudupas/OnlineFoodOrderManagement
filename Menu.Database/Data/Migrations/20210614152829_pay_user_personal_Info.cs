using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuisnessLayer.Migrations
{
    public partial class pay_user_personal_Info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "tblUser",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "tblUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "tblUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "tblUser",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblPaymentMode",
                columns: table => new
                {
                    PaymentModeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymenentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPaymentMode", x => x.PaymentModeId);
                });

            migrationBuilder.CreateTable(
                name: "tblState",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateNames = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblState", x => x.StateId);
                });

            migrationBuilder.CreateTable(
                name: "tblCity",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityNames = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCity", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_tblCity_tblState_StateId",
                        column: x => x.StateId,
                        principalTable: "tblState",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCity_StateId",
                table: "tblCity",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCity");

            migrationBuilder.DropTable(
                name: "tblPaymentMode");

            migrationBuilder.DropTable(
                name: "tblState");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "tblUser");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "tblUser");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "tblUser");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "tblUser",
                newName: "Nickname");
        }
    }
}
