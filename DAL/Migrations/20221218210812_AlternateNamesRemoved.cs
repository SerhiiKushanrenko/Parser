using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    public partial class AlternateNamesRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlternateNames");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ScientistsSocialNetworks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ScientistsSocialNetworks");

            migrationBuilder.CreateTable(
                name: "AlternateNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScientistId = table.Column<int>(type: "integer", nullable: false),
                    ScientistSocialNetworkId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
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
        }
    }
}
