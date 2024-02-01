using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHospital.Letters.Context.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Sections",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Sections");
        }
    }
}
