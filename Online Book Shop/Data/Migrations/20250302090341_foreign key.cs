using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Book_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class foreignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Book_Id",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Order_Id",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderStatus_Id",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Book_Id",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "Genre_Id",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "ShoppingCart",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Status_Id",
                table: "OrderStatus",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "Quantity_Id",
                table: "OrderDetails",
                newName: "QuantityId");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Order",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShoppingCart",
                newName: "User_Id");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "OrderStatus",
                newName: "Status_Id");

            migrationBuilder.RenameColumn(
                name: "QuantityId",
                table: "OrderDetails",
                newName: "Quantity_Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Order",
                newName: "User_Id");

            migrationBuilder.AddColumn<int>(
                name: "Book_Id",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order_Id",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus_Id",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Book_Id",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Genre_Id",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
