using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHospital.Letters.Context.Migrations
{
    /// <inheritdoc />
    public partial class SectionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultTitle",
                table: "SectionTypes",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SortOrder",
                table: "Sections",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_LetterTemplatesSectionTemplates_SortOrder",
                table: "LetterTemplatesSectionTemplates",
                column: "SortOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sections_SortOrder",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_LetterTemplatesSectionTemplates_SortOrder",
                table: "LetterTemplatesSectionTemplates");

            migrationBuilder.DropColumn(
                name: "DefaultTitle",
                table: "SectionTypes");
        }
    }
}
