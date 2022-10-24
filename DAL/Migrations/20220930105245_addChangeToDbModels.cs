using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class addChangeToDbModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOfScientists_Scientists_scientistId",
                table: "WorkOfScientists");

            migrationBuilder.DropIndex(
                name: "IX_WorkOfScientists_scientistId",
                table: "WorkOfScientists");

            migrationBuilder.CreateTable(
                name: "ScientistWorkOfScientist",
                columns: table => new
                {
                    ScientistsId = table.Column<int>(type: "integer", nullable: false),
                    WorkOfScientistsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientistWorkOfScientist", x => new { x.ScientistsId, x.WorkOfScientistsId });
                    table.ForeignKey(
                        name: "FK_ScientistWorkOfScientist_Scientists_ScientistsId",
                        column: x => x.ScientistsId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScientistWorkOfScientist_WorkOfScientists_WorkOfScientistsId",
                        column: x => x.WorkOfScientistsId,
                        principalTable: "WorkOfScientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScientistWorkOfScientist_WorkOfScientistsId",
                table: "ScientistWorkOfScientist",
                column: "WorkOfScientistsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientistWorkOfScientist");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOfScientists_scientistId",
                table: "WorkOfScientists",
                column: "scientistId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOfScientists_Scientists_scientistId",
                table: "WorkOfScientists",
                column: "scientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
