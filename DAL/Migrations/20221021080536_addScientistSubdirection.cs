using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class addScientistSubdirection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_SubdirectionOfWorks_SubdirectioOsWorkId",
                table: "Scientists");

            migrationBuilder.DropIndex(
                name: "IX_Scientists_SubdirectioOsWorkId",
                table: "Scientists");

            migrationBuilder.DropColumn(
                name: "SubdirectioOsWorkId",
                table: "Scientists");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SubdirectionOfWorks",
                newName: "Title");

            migrationBuilder.CreateTable(
                name: "ScientistSubdirection",
                columns: table => new
                {
                    ScientistId = table.Column<int>(type: "integer", nullable: false),
                    SubdirectionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientistSubdirection", x => new { x.ScientistId, x.SubdirectionId });
                    table.ForeignKey(
                        name: "FK_ScientistSubdirection_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScientistSubdirection_SubdirectionOfWorks_SubdirectionId",
                        column: x => x.SubdirectionId,
                        principalTable: "SubdirectionOfWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScientistSubdirection_SubdirectionId",
                table: "ScientistSubdirection",
                column: "SubdirectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientistSubdirection");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "SubdirectionOfWorks",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "SubdirectioOsWorkId",
                table: "Scientists",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scientists_SubdirectioOsWorkId",
                table: "Scientists",
                column: "SubdirectioOsWorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_SubdirectionOfWorks_SubdirectioOsWorkId",
                table: "Scientists",
                column: "SubdirectioOsWorkId",
                principalTable: "SubdirectionOfWorks",
                principalColumn: "Id");
        }
    }
}
