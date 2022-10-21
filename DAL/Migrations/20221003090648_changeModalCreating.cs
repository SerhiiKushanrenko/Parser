using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parser1.Migrations
{
    public partial class changeModalCreating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientistWorkOfScientist");

            migrationBuilder.DropIndex(
                name: "IX_ScientistsWork_ScientistId",
                table: "ScientistsWork");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork",
                columns: new[] { "ScientistId", "WorkOfScientistId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork");

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
                name: "IX_ScientistsWork_ScientistId",
                table: "ScientistsWork",
                column: "ScientistId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientistWorkOfScientist_WorkOfScientistsId",
                table: "ScientistWorkOfScientist",
                column: "WorkOfScientistsId");
        }
    }
}
