using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReservationToTableOnetoMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_RestaurantTableId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RestaurantTableId",
                table: "Reservations",
                column: "RestaurantTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_RestaurantTableId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RestaurantTableId",
                table: "Reservations",
                column: "RestaurantTableId",
                unique: true);
        }
    }
}
