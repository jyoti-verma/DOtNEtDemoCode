using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHospital.Letters.Context.Migrations
{
    /// <inheritdoc />
    public partial class RenamedToTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Snippets",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Snippets",
                newName: "Name");
        }
    }
}
