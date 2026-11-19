using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZParadiseAPI.Model.Migrations
{
    /// <inheritdoc />
    public partial class PropertyTypeAttachmentAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PropertyTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "PropertyFeatures",
                columns: table => new
                {
                    PropertyFeatureID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyFeatures", x => x.PropertyFeatureID);
                });

            migrationBuilder.InsertData(
                table: "PropertyFeatures",
                columns: new[] { "PropertyFeatureID", "Description" },
                values: new object[,]
                {
                    { 1, "Gas" },
                    { 2, "Washing Machine" },
                    { 3, "Dryer" },
                    { 4, "Electricity" },
                    { 5, "Cold Water" },
                    { 6, "Hot Water" },
                    { 7, "Air Conditioner" },
                    { 8, "Heater" },
                    { 9, "Swimming Pool" },
                    { 10, "GYM" },
                    { 20, "Other" }
                });

            migrationBuilder.InsertData(
                table: "PropertyTypes",
                columns: new[] { "PropertyTypeID", "Description" },
                values: new object[,]
                {
                    { 1, "Apartment" },
                    { 2, "Condominium" },
                    { 3, "Mansions" },
                    { 4, "Room" },
                    { 5, "Landed House" },
                    { 6, "Townhouse" },
                    { 20, "Other" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyFeatures");

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PropertyTypes",
                keyColumn: "PropertyTypeID",
                keyValue: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PropertyTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
