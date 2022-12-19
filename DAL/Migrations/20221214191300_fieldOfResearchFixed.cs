using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class fieldOfResearchFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concept_Scientists_ScientistId",
                table: "Concept");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldOfResearch_FieldOfResearch_ParentFieldOfResearchId",
                table: "FieldOfResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistFieldOfResearch_FieldOfResearch_FieldOfResearchId",
                table: "ScientistFieldOfResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistFieldOfResearch_Scientists_ScientistId",
                table: "ScientistFieldOfResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_Organization_OrganizationId",
                table: "Scientists");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialNetworkOfScientists_Scientists_ScientistId",
                table: "SocialNetworkOfScientists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialNetworkOfScientists",
                table: "SocialNetworkOfScientists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistFieldOfResearch",
                table: "ScientistFieldOfResearch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organization",
                table: "Organization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FieldOfResearch",
                table: "FieldOfResearch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Concept",
                table: "Concept");

            migrationBuilder.RenameTable(
                name: "SocialNetworkOfScientists",
                newName: "ScientistsSocialNetworks");

            migrationBuilder.RenameTable(
                name: "ScientistFieldOfResearch",
                newName: "ScientistsFieldsOfResearch");

            migrationBuilder.RenameTable(
                name: "Organization",
                newName: "Organizations");

            migrationBuilder.RenameTable(
                name: "FieldOfResearch",
                newName: "FieldsOfResearch");

            migrationBuilder.RenameTable(
                name: "Concept",
                newName: "Concepts");

            migrationBuilder.RenameIndex(
                name: "IX_SocialNetworkOfScientists_ScientistId",
                table: "ScientistsSocialNetworks",
                newName: "IX_ScientistsSocialNetworks_ScientistId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistFieldOfResearch_ScientistId",
                table: "ScientistsFieldsOfResearch",
                newName: "IX_ScientistsFieldsOfResearch_ScientistId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistFieldOfResearch_FieldOfResearchId",
                table: "ScientistsFieldsOfResearch",
                newName: "IX_ScientistsFieldsOfResearch_FieldOfResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_FieldOfResearch_ParentFieldOfResearchId",
                table: "FieldsOfResearch",
                newName: "IX_FieldsOfResearch_ParentFieldOfResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_Concept_ScientistId",
                table: "Concepts",
                newName: "IX_Concepts_ScientistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistsSocialNetworks",
                table: "ScientistsSocialNetworks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistsFieldsOfResearch",
                table: "ScientistsFieldsOfResearch",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FieldsOfResearch",
                table: "FieldsOfResearch",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Concepts",
                table: "Concepts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Concepts_Scientists_ScientistId",
                table: "Concepts",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldsOfResearch_FieldsOfResearch_ParentFieldOfResearchId",
                table: "FieldsOfResearch",
                column: "ParentFieldOfResearchId",
                principalTable: "FieldsOfResearch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_Organizations_OrganizationId",
                table: "Scientists",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistsFieldsOfResearch_FieldsOfResearch_FieldOfResearch~",
                table: "ScientistsFieldsOfResearch",
                column: "FieldOfResearchId",
                principalTable: "FieldsOfResearch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistsFieldsOfResearch_Scientists_ScientistId",
                table: "ScientistsFieldsOfResearch",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistsSocialNetworks_Scientists_ScientistId",
                table: "ScientistsSocialNetworks",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concepts_Scientists_ScientistId",
                table: "Concepts");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldsOfResearch_FieldsOfResearch_ParentFieldOfResearchId",
                table: "FieldsOfResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_Organizations_OrganizationId",
                table: "Scientists");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistsFieldsOfResearch_FieldsOfResearch_FieldOfResearch~",
                table: "ScientistsFieldsOfResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistsFieldsOfResearch_Scientists_ScientistId",
                table: "ScientistsFieldsOfResearch");

            migrationBuilder.DropForeignKey(
                name: "FK_ScientistsSocialNetworks_Scientists_ScientistId",
                table: "ScientistsSocialNetworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistsSocialNetworks",
                table: "ScientistsSocialNetworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScientistsFieldsOfResearch",
                table: "ScientistsFieldsOfResearch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FieldsOfResearch",
                table: "FieldsOfResearch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Concepts",
                table: "Concepts");

            migrationBuilder.RenameTable(
                name: "ScientistsSocialNetworks",
                newName: "SocialNetworkOfScientists");

            migrationBuilder.RenameTable(
                name: "ScientistsFieldsOfResearch",
                newName: "ScientistFieldOfResearch");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "Organization");

            migrationBuilder.RenameTable(
                name: "FieldsOfResearch",
                newName: "FieldOfResearch");

            migrationBuilder.RenameTable(
                name: "Concepts",
                newName: "Concept");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistsSocialNetworks_ScientistId",
                table: "SocialNetworkOfScientists",
                newName: "IX_SocialNetworkOfScientists_ScientistId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistsFieldsOfResearch_ScientistId",
                table: "ScientistFieldOfResearch",
                newName: "IX_ScientistFieldOfResearch_ScientistId");

            migrationBuilder.RenameIndex(
                name: "IX_ScientistsFieldsOfResearch_FieldOfResearchId",
                table: "ScientistFieldOfResearch",
                newName: "IX_ScientistFieldOfResearch_FieldOfResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_FieldsOfResearch_ParentFieldOfResearchId",
                table: "FieldOfResearch",
                newName: "IX_FieldOfResearch_ParentFieldOfResearchId");

            migrationBuilder.RenameIndex(
                name: "IX_Concepts_ScientistId",
                table: "Concept",
                newName: "IX_Concept_ScientistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialNetworkOfScientists",
                table: "SocialNetworkOfScientists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScientistFieldOfResearch",
                table: "ScientistFieldOfResearch",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organization",
                table: "Organization",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FieldOfResearch",
                table: "FieldOfResearch",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Concept",
                table: "Concept",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Concept_Scientists_ScientistId",
                table: "Concept",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldOfResearch_FieldOfResearch_ParentFieldOfResearchId",
                table: "FieldOfResearch",
                column: "ParentFieldOfResearchId",
                principalTable: "FieldOfResearch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistFieldOfResearch_FieldOfResearch_FieldOfResearchId",
                table: "ScientistFieldOfResearch",
                column: "FieldOfResearchId",
                principalTable: "FieldOfResearch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScientistFieldOfResearch_Scientists_ScientistId",
                table: "ScientistFieldOfResearch",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_Organization_OrganizationId",
                table: "Scientists",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialNetworkOfScientists_Scientists_ScientistId",
                table: "SocialNetworkOfScientists",
                column: "ScientistId",
                principalTable: "Scientists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
