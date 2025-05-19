using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMinDistanceDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MinDistance",
                table: "Contacts",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "MinDistance",
                table: "Contacts",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
