using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuisnessLayer.Migrations
{
    public partial class tableaddtblUserandsomemodification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CartAmount",
                table: "tblUser",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "tblUser",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<long>(
                name: "Points",
                table: "tblUser",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "tblUser",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "tblPaymentType",
                columns: table => new
                {
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPaymentType", x => x.PaymentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "tblUserOrder",
                columns: table => new
                {
                    UserOrderId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserOrder", x => x.UserOrderId);
                    table.ForeignKey(
                        name: "FK_tblUserOrder_tblMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "tblMenu",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserOrder_tblUser_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserOrder_tblVendorList_VendorId",
                        column: x => x.VendorId,
                        principalTable: "tblVendorList",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblUserOrder_MenuId",
                table: "tblUserOrder",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserOrder_UserId",
                table: "tblUserOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserOrder_VendorId",
                table: "tblUserOrder",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblPaymentType");

            migrationBuilder.DropTable(
                name: "tblUserOrder");

            migrationBuilder.DropColumn(
                name: "CartAmount",
                table: "tblUser");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "tblUser");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "tblUser");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "tblUser");
        }
    }
}
