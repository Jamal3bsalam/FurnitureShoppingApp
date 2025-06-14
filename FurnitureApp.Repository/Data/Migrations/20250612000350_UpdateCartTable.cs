using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurnitureApp.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryMethodId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "DeliveryMethodId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Carts");
        }
    }
}
