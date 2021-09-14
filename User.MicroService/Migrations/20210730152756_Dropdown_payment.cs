using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.MicroService.Migrations
{
    public partial class Dropdown_payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentDropDown",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDropDown", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PaymentDropDown",
                columns: new[] { "Id", "Code", "Label", "Value" },
                values: new object[,]
                {
                    { 1L, null, "Credit Card", "Credit Card" },
                    { 2L, null, "UPI", "UPI" },
                    { 3L, null, "Debit Card", "Debit Card" },
                    { 4L, null, "Wallet", "Wallet" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDropDown");
        }
    }
}
