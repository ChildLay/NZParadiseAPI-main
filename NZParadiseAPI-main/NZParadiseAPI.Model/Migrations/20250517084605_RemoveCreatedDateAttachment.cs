using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZParadiseAPI.Model.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreatedDateAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Attachments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Attachments",
                type: "datetime2",
                nullable: true);
        }
    }
}
