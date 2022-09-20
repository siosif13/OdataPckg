using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdataPckg.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Posts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "Blogs",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Posts",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Blogs",
                newName: "BlogId");
        }
    }
}
