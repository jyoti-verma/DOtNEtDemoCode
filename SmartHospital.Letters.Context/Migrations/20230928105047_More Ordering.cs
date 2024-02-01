using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHospital.Letters.Context.Migrations
{
    /// <inheritdoc />
    public partial class MoreOrdering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Values",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "SnippetTemplates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Snippets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "SnippetTemplates");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Snippets");
        }
    }
}
