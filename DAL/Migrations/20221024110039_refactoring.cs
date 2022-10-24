using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    public partial class refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_Directions_DirectionId",
                table: "Scientists");

            migrationBuilder.DropTable(
                name: "ScientistSubdirections");

            migrationBuilder.DropTable(
                name: "Subdirections");

            migrationBuilder.DropTable(
                name: "Directions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork");

            migrationBuilder.DropIndex(
                name: "IX_Scientists_DirectionId",
                table: "Scientists");

            migrationBuilder.DropColumn(
                name: "DirectionId",
                table: "Scientists");

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworkScientistId",
                table: "SocialNetworkOfScientists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "SocialNetworkOfScientists",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ScientistsWork",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FieldOfResearch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ANZSRC = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ParentFieldOfResearchId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOfResearch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldOfResearch_FieldOfResearch_ParentFieldOfResearchId",
                        column: x => x.ParentFieldOfResearchId,
                        principalTable: "FieldOfResearch",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScientistFieldOfResearch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScientistId = table.Column<int>(type: "integer", nullable: false),
                    FieldOfResearchId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientistFieldOfResearch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScientistFieldOfResearch_FieldOfResearch_FieldOfResearchId",
                        column: x => x.FieldOfResearchId,
                        principalTable: "FieldOfResearch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScientistFieldOfResearch_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScientistsWork_ScientistId",
                table: "ScientistsWork",
                column: "ScientistId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldOfResearch_ParentFieldOfResearchId",
                table: "FieldOfResearch",
                column: "ParentFieldOfResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientistFieldOfResearch_FieldOfResearchId",
                table: "ScientistFieldOfResearch",
                column: "FieldOfResearchId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientistFieldOfResearch_ScientistId",
                table: "ScientistFieldOfResearch",
                column: "ScientistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientistFieldOfResearch");

            migrationBuilder.DropTable(
                name: "FieldOfResearch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork");

            migrationBuilder.DropIndex(
                name: "IX_ScientistsWork_ScientistId",
                table: "ScientistsWork");

            migrationBuilder.DropColumn(
                name: "SocialNetworkScientistId",
                table: "SocialNetworkOfScientists");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SocialNetworkOfScientists");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ScientistsWork");

            migrationBuilder.AddColumn<int>(
                name: "DirectionId",
                table: "Scientists",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistsWork",
                table: "ScientistsWork",
                columns: new[] { "ScientistId", "WorkId" });

            migrationBuilder.CreateTable(
                name: "Directions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subdirections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DirectionId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subdirections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subdirections_Directions_DirectionId",
                        column: x => x.DirectionId,
                        principalTable: "Directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScientistSubdirections",
                columns: table => new
                {
                    ScientistId = table.Column<int>(type: "integer", nullable: false),
                    SubdirectionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientistSubdirections", x => new { x.ScientistId, x.SubdirectionId });
                    table.ForeignKey(
                        name: "FK_ScientistSubdirections_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScientistSubdirections_Subdirections_SubdirectionId",
                        column: x => x.SubdirectionId,
                        principalTable: "Subdirections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scientists_DirectionId",
                table: "Scientists",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScientistSubdirections_SubdirectionId",
                table: "ScientistSubdirections",
                column: "SubdirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subdirections_DirectionId",
                table: "Subdirections",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_Directions_DirectionId",
                table: "Scientists",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
