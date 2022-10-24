using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    public partial class OrganisationIdAddedToScientist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NetworkOfScientists_Scientists_ScientistId",
                table: "NetworkOfScientists");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistSubdirections_Subdirection_SubdirectionId",
                table: "ScientistSubdirections");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistsWork_WorkOfScientists_WorkOfScientistId",
                table: "ScientistsWork");

            migrationBuilder.DropForeignKey(
                name: "FK_Subdirection_Directions_DirectionId",
                table: "Subdirection");

            migrationBuilder.DropTable(
                name: "WorkOfScientists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subdirection",
                table: "Subdirection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetworkOfScientists",
                table: "NetworkOfScientists");

            migrationBuilder.DropColumn(
                name: "Organization",
                table: "Scientists");

            migrationBuilder.RenameTable(
                name: "Subdirection",
                newName: "Subdirections");

            migrationBuilder.RenameTable(
                name: "NetworkOfScientists",
                newName: "SocialNetworkOfScientists");

            migrationBuilder.RenameColumn(
                name: "WorkOfScientistId",
                table: "ScientistsWork",
                newName: "WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistsWork_WorkOfScientistId",
                table: "ScientistsWork",
                newName: "IX_ScientistsWork_WorkId");

            migrationBuilder.RenameIndex(
                name: "IX_Subdirection_DirectionId",
                table: "Subdirections",
                newName: "IX_Subdirections_DirectionId");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkOfScientists_ScientistId",
                table: "SocialNetworkOfScientists",
                newName: "IX_SocialNetworkOfScientists_ScientistId");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Scientists",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subdirections",
                table: "Subdirections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialNetworkOfScientists",
                table: "SocialNetworkOfScientists",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scientists_OrganizationId",
                table: "Scientists",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_Organization_OrganizationId",
                table: "Scientists",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistSubdirections_Subdirections_SubdirectionId",
                table: "ScientistSubdirections",
                column: "SubdirectionId",
                principalTable: "Subdirections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistsWork_Works_WorkId",
                table: "ScientistsWork",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialNetworkOfScientists_Scientists_ScientistId",
                table: "SocialNetworkOfScientists",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subdirections_Directions_DirectionId",
                table: "Subdirections",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_Organization_OrganizationId",
                table: "Scientists");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistSubdirections_Subdirections_SubdirectionId",
                table: "ScientistSubdirections");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistsWork_Works_WorkId",
                table: "ScientistsWork");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialNetworkOfScientists_Scientists_ScientistId",
                table: "SocialNetworkOfScientists");

            migrationBuilder.DropForeignKey(
                name: "FK_Subdirections_Directions_DirectionId",
                table: "Subdirections");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Scientists_OrganizationId",
                table: "Scientists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subdirections",
                table: "Subdirections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialNetworkOfScientists",
                table: "SocialNetworkOfScientists");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Scientists");

            migrationBuilder.RenameTable(
                name: "Subdirections",
                newName: "Subdirection");

            migrationBuilder.RenameTable(
                name: "SocialNetworkOfScientists",
                newName: "NetworkOfScientists");

            migrationBuilder.RenameColumn(
                name: "WorkId",
                table: "ScientistsWork",
                newName: "WorkOfScientistId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistsWork_WorkId",
                table: "ScientistsWork",
                newName: "IX_ScientistsWork_WorkOfScientistId");

            migrationBuilder.RenameIndex(
                name: "IX_Subdirections_DirectionId",
                table: "Subdirection",
                newName: "IX_Subdirection_DirectionId");

            migrationBuilder.RenameIndex(
                name: "IX_SocialNetworkOfScientists_ScientistId",
                table: "NetworkOfScientists",
                newName: "IX_NetworkOfScientists_ScientistId");

            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "Scientists",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subdirection",
                table: "Subdirection",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetworkOfScientists",
                table: "NetworkOfScientists",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WorkOfScientists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOfScientists", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_NetworkOfScientists_Scientists_ScientistId",
                table: "NetworkOfScientists",
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
                name: "FK_ScientistsWork_WorkOfScientists_WorkOfScientistId",
                table: "ScientistsWork",
                column: "WorkOfScientistId",
                principalTable: "WorkOfScientists",
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
    }
}
