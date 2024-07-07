using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepartmentEmployees.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTheFieldNameInModelsFromPostToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Post",
                table: "Employees",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Employees",
                newName: "Post");
        }
    }
}
