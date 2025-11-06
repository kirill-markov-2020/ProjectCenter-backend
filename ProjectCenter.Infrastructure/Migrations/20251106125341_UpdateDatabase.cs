using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Comment_CommentId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_CommentId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Project");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProjectId",
                table: "Comment",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Project_ProjectId",
                table: "Comment",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Project_ProjectId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ProjectId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Project",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_CommentId",
                table: "Project",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Comment_CommentId",
                table: "Project",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
