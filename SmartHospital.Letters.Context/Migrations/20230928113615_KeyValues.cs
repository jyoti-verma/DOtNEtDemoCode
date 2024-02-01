using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHospital.Letters.Context.Migrations
{
    /// <inheritdoc />
    public partial class KeyValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyValues",
                table: "SnippetTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyValues",
                table: "SnippetTemplates");
        }
    }
}
