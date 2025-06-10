using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC0619Final_tempera.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCompanyField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCompany",
                table: "ProductTable",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCompany",
                table: "ProductTable");
        }
    }
}
