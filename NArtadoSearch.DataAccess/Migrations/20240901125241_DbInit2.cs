using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NArtadoSearch.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DbInit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "IndexedWebUrls",
                newName: "ArticlesContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArticlesContent",
                table: "IndexedWebUrls",
                newName: "Tags");
        }
    }
}
