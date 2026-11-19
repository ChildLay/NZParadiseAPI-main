using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZParadiseAPI.Model.Migrations
{
    /// <inheritdoc />
    public partial class PropertyTypeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 3,
                column: "Description",
                value: "Mansion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 3,
                column: "Description",
                value: "Mansions");
        }
    }
}
