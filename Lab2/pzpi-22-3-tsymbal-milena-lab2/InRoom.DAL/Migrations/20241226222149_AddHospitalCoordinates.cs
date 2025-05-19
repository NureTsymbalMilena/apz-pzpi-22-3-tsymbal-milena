using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddHospitalCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "X",
                table: "Hospitals",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Y",
                table: "Hospitals",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Z",
                table: "Hospitals",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Z",
                table: "Hospitals");
        }
    }
}
