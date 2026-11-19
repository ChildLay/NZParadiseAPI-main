using Microsoft.EntityFrameworkCore.Migrations;

namespace NZParadiseAPI.Model.Migrations
{
    public partial class Messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing FK_Properties_Users_UserID if it uses CASCADE
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_UserID",
                table: "Properties");

            // Recreate FK_Properties_Users_UserID with NoAction
            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_UserID",
                table: "Properties",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.NoAction);

            // Create Messages table
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    FromUserID = table.Column<int>(type: "int", nullable: false),
                    MessageDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Messages_Properties_PropertyID",
                        column: x => x.PropertyID,
                        principalTable: "Properties",
                        principalColumn: "PropertyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_FromUserID",
                        column: x => x.FromUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ParentID",
                table: "Attachments",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUserID",
                table: "Messages",
                column: "FromUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PropertyID",
                table: "Messages",
                column: "PropertyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Properties_ParentID",
                table: "Attachments",
                column: "ParentID",
                principalTable: "Properties",
                principalColumn: "PropertyID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Properties_ParentID",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_UserID",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ParentID",
                table: "Attachments");

            // Recreate FK_Properties_Users_UserID as it was (assuming CASCADE)
            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_UserID",
                table: "Properties",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}