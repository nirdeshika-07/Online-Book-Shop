using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

namespace Online_Book_Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnitPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove this code that's causing the error
            // migrationBuilder.DropColumn(
            //     name: "ShoppingCart_Id",
            //     table: "CartDetails");

            migrationBuilder.AddColumn<double>(
                name: "UnitPrice",
                table: "CartDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "CartDetails");

            // Remove this code too since we're not adding the column in Up()
            // migrationBuilder.AddColumn<int>(
            //     name: "ShoppingCart_Id",
            //     table: "CartDetails",
            //     type: "int",
            //     nullable: false,
            //     defaultValue: 0);
        }
    }
}