using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    public partial class AlternateNamesadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScientistsWork_Scientists_ScientistId",
                table: "ScientistsWork");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistsWork_Works_WorkId",
                table: "ScientistsWork");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork");

            migrationBuilder.RenameTable(
                name: "ScientistsWork",
                newName: "ScientistWorks");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistsWork_WorkId",
                table: "ScientistWorks",
                newName: "IX_ScientistWorks_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistsWork_ScientistId",
                table: "ScientistWorks",
                newName: "IX_ScientistWorks_ScientistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistWorks",
                table: "ScientistWorks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AlternateNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScientistSocialNetworkId = table.Column<int>(type: "integer", nullable: false),
                    ScientistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternateNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlternateNames_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlternateNames_ScientistsSocialNetworks_ScientistSocialNetw~",
                        column: x => x.ScientistSocialNetworkId,
                        principalTable: "ScientistsSocialNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlternateNames_ScientistId",
                table: "AlternateNames",
                column: "ScientistId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternateNames_ScientistSocialNetworkId",
                table: "AlternateNames",
                column: "ScientistSocialNetworkId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistWorks_Scientists_ScientistId",
                table: "ScientistWorks",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistWorks_Works_WorkId",
                table: "ScientistWorks",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScientistWorks_Scientists_ScientistId",
                table: "ScientistWorks");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistWorks_Works_WorkId",
                table: "ScientistWorks");

            migrationBuilder.DropTable(
                name: "AlternateNames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistWorks",
                table: "ScientistWorks");

            migrationBuilder.RenameTable(
                name: "ScientistWorks",
                newName: "ScientistsWork");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistWorks_WorkId",
                table: "ScientistsWork",
                newName: "IX_ScientistsWork_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistWorks_ScientistId",
                table: "ScientistsWork",
                newName: "IX_ScientistsWork_ScientistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistsWork_Scientists_ScientistId",
                table: "ScientistsWork",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistsWork_Works_WorkId",
                table: "ScientistsWork",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
