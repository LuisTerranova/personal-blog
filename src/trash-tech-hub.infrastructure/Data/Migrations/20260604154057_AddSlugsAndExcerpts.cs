using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trash_tech_hub.infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSlugsAndExcerpts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Projects",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "Posts",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Posts",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE \"Posts\" SET \"Slug\" = LOWER(REGEXP_REPLACE(REGEXP_REPLACE(TRIM(\"Title\"), '[^a-zA-Z0-9\\s-]', '', 'g'), '\\s+', '-', 'g')) || '-' || \"Id\" WHERE \"Slug\" = '';");
            migrationBuilder.Sql("UPDATE \"Posts\" SET \"Excerpt\" = SUBSTRING(\"Body\" FROM 1 FOR 250) WHERE \"Excerpt\" = '';");
            migrationBuilder.Sql("UPDATE \"Projects\" SET \"Slug\" = LOWER(REGEXP_REPLACE(REGEXP_REPLACE(TRIM(\"Title\"), '[^a-zA-Z0-9\\s-]', '', 'g'), '\\s+', '-', 'g')) || '-' || \"Id\" WHERE \"Slug\" = '';");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Slug",
                table: "Projects",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Slug",
                table: "Posts",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_Slug",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Slug",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Posts");
        }
    }
}
