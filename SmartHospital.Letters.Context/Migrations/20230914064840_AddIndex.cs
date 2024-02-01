using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHospital.Letters.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sections_SortOrder",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_LetterTemplatesSectionTemplates_SortOrder",
                table: "LetterTemplatesSectionTemplates");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SortOrder_Id",
                table: "Sections",
                columns: new[] { "SortOrder", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_LetterTemplatesSectionTemplates_SortOrder_Id",
                table: "LetterTemplatesSectionTemplates",
                columns: new[] { "SortOrder", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sections_SortOrder_Id",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_LetterTemplatesSectionTemplates_SortOrder_Id",
                table: "LetterTemplatesSectionTemplates");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SortOrder",
                table: "Sections",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_LetterTemplatesSectionTemplates_SortOrder",
                table: "LetterTemplatesSectionTemplates",
                column: "SortOrder");
        }
    }
}
