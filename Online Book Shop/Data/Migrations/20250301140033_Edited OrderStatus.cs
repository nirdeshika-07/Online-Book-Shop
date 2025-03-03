using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Book_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditedOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status_Id",
                table: "OrderStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status_Id",
                table: "OrderStatus");
        }
    }
}
