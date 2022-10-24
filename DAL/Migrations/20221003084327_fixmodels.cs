using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class fixmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScientistsWork",
                columns: table => new
                {
                    ScientistId = table.Column<int>(type: "integer", nullable: false),
                    WorkOfScientistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ScientistsWork_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScientistsWork_WorkOfScientists_WorkOfScientistId",
                        column: x => x.WorkOfScientistId,
                        principalTable: "WorkOfScientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScientistsWork_ScientistId",
                table: "ScientistsWork",
                column: "ScientistId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientistsWork_WorkOfScientistId",
                table: "ScientistsWork",
                column: "WorkOfScientistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientistsWork");
        }
    }
}
