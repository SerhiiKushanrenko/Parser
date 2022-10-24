using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Parser.Migrations
{
    public partial class addScientistNetwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NetworkOfScientists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkOfScientists", x => x.Id);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientistNetworks");

            migrationBuilder.DropTable(
                name: "NetworkOfScientists");
        }
    }
}
