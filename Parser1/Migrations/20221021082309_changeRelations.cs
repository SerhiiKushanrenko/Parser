using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parser1.Migrations
{
    public partial class changeRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScientistSubdirection_Scientists_ScientistId",
                table: "ScientistSubdirection");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistSubdirection_SubdirectionOfWorks_SubdirectionId",
                table: "ScientistSubdirection");

            migrationBuilder.DropForeignKey(
                name: "FK_SubdirectionOfWorks_Directions_DirectionId",
                table: "SubdirectionOfWorks");

            migrationBuilder.DropTable(
                name: "ScientistNetworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubdirectionOfWorks",
                table: "SubdirectionOfWorks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistSubdirection",
                table: "ScientistSubdirection");

            migrationBuilder.RenameTable(
                name: "SubdirectionOfWorks",
                newName: "Subdirection");

            migrationBuilder.RenameTable(
                name: "ScientistSubdirection",
                newName: "ScientistSubdirections");

            migrationBuilder.RenameIndex(
                name: "IX_SubdirectionOfWorks_DirectionId",
                table: "Subdirection",
                newName: "IX_Subdirection_DirectionId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistSubdirection_SubdirectionId",
                table: "ScientistSubdirections",
                newName: "IX_ScientistSubdirections_SubdirectionId");

            migrationBuilder.AddColumn<int>(
                name: "ScientistId",
                table: "NetworkOfScientists",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subdirection",
                table: "Subdirection",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistSubdirections",
                table: "ScientistSubdirections",
                columns: new[] { "ScientistId", "SubdirectionId" });

            migrationBuilder.CreateIndex(
                name: "IX_NetworkOfScientists_ScientistId",
                table: "NetworkOfScientists",
                column: "ScientistId");

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkOfScientists_Scientists_ScientistId",
                table: "NetworkOfScientists",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistSubdirections_Scientists_ScientistId",
                table: "ScientistSubdirections",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistSubdirections_Subdirection_SubdirectionId",
                table: "ScientistSubdirections",
                column: "SubdirectionId",
                principalTable: "Subdirection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subdirection_Directions_DirectionId",
                table: "Subdirection",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NetworkOfScientists_Scientists_ScientistId",
                table: "NetworkOfScientists");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistSubdirections_Scientists_ScientistId",
                table: "ScientistSubdirections");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistSubdirections_Subdirection_SubdirectionId",
                table: "ScientistSubdirections");

            migrationBuilder.DropForeignKey(
                name: "FK_Subdirection_Directions_DirectionId",
                table: "Subdirection");

            migrationBuilder.DropIndex(
                name: "IX_NetworkOfScientists_ScientistId",
                table: "NetworkOfScientists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subdirection",
                table: "Subdirection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistSubdirections",
                table: "ScientistSubdirections");

            migrationBuilder.DropColumn(
                name: "ScientistId",
                table: "NetworkOfScientists");

            migrationBuilder.RenameTable(
                name: "Subdirection",
                newName: "SubdirectionOfWorks");

            migrationBuilder.RenameTable(
                name: "ScientistSubdirections",
                newName: "ScientistSubdirection");

            migrationBuilder.RenameIndex(
                name: "IX_Subdirection_DirectionId",
                table: "SubdirectionOfWorks",
                newName: "IX_SubdirectionOfWorks_DirectionId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistSubdirections_SubdirectionId",
                table: "ScientistSubdirection",
                newName: "IX_ScientistSubdirection_SubdirectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubdirectionOfWorks",
                table: "SubdirectionOfWorks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistSubdirection",
                table: "ScientistSubdirection",
                columns: new[] { "ScientistId", "SubdirectionId" });

            migrationBuilder.CreateTable(
                name: "ScientistNetworks",
                columns: table => new
                {
                    ScientistId = table.Column<int>(type: "integer", nullable: false),
                    SocialNetworkOfScientistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientistNetworks", x => new { x.ScientistId, x.SocialNetworkOfScientistId });
                    table.ForeignKey(
                        name: "FK_ScientistNetworks_NetworkOfScientists_SocialNetworkOfScient~",
                        column: x => x.SocialNetworkOfScientistId,
                        principalTable: "NetworkOfScientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScientistNetworks_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScientistNetworks_SocialNetworkOfScientistId",
                table: "ScientistNetworks",
                column: "SocialNetworkOfScientistId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistSubdirection_Scientists_ScientistId",
                table: "ScientistSubdirection",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistSubdirection_SubdirectionOfWorks_SubdirectionId",
                table: "ScientistSubdirection",
                column: "SubdirectionId",
                principalTable: "SubdirectionOfWorks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubdirectionOfWorks_Directions_DirectionId",
                table: "SubdirectionOfWorks",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
