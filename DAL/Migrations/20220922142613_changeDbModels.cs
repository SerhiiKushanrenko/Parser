using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parser.Migrations
{
    public partial class changeDbModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_Directions_DirectionId",
                table: "Scientists");

            migrationBuilder.AlterColumn<int>(
                name: "DirectionId",
                table: "Scientists",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_Directions_DirectionId",
                table: "Scientists",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scientists_Directions_DirectionId",
                table: "Scientists");

            migrationBuilder.AlterColumn<int>(
                name: "DirectionId",
                table: "Scientists",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Scientists_Directions_DirectionId",
                table: "Scientists",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id");
        }
    }
}
