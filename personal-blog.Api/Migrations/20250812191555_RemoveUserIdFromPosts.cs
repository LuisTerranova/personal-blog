using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace personal_blog.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdFromPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Projects",
                type: "BIGINT",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
