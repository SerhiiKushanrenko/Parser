using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class addfieldtoScietist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubdirectionOfWork_Directions_DirectionId",
                table: "SubdirectionOfWork");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubdirectionOfWork",
                table: "SubdirectionOfWork");

            migrationBuilder.RenameTable(
                name: "SubdirectionOfWork",
                newName: "SubdirectionOfWorks");

            migrationBuilder.RenameColumn(
                name: "SubdirectionOfWorkId",
                table: "SubdirectionOfWorks",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_SubdirectionOfWork_DirectionId",
                table: "SubdirectionOfWorks",
                newName: "IX_SubdirectionOfWorks_DirectionId");

            migrationBuilder.AddColumn<int>(
                name: "SubdirectioOsWorkId",
                table: "Scientists",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubdirectionOfWorks",
                table: "SubdirectionOfWorks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Scientists_SubdirectioOsWorkId",
                table: "Scientists",
                column: "SubdirectioOsWorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_SubdirectionOfWorks_SubdirectioOsWorkId",
                table: "Scientists",
                column: "SubdirectioOsWorkId",
                principalTable: "SubdirectionOfWorks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubdirectionOfWorks_Directions_DirectionId",
                table: "SubdirectionOfWorks",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_SubdirectionOfWorks_SubdirectioOsWorkId",
                table: "Scientists");

            migrationBuilder.DropForeignKey(
                name: "FK_SubdirectionOfWorks_Directions_DirectionId",
                table: "SubdirectionOfWorks");

            migrationBuilder.DropIndex(
                name: "IX_Scientists_SubdirectioOsWorkId",
                table: "Scientists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubdirectionOfWorks",
                table: "SubdirectionOfWorks");

            migrationBuilder.DropColumn(
                name: "SubdirectioOsWorkId",
                table: "Scientists");

            migrationBuilder.RenameTable(
                name: "SubdirectionOfWorks",
                newName: "SubdirectionOfWork");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SubdirectionOfWork",
                newName: "SubdirectionOfWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_SubdirectionOfWorks_DirectionId",
                table: "SubdirectionOfWork",
                newName: "IX_SubdirectionOfWork_DirectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubdirectionOfWork",
                table: "SubdirectionOfWork",
                column: "SubdirectionOfWorkId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubdirectionOfWork_Directions_DirectionId",
                table: "SubdirectionOfWork",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
