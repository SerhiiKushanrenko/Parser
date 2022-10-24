using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parser.Migrations
{
    public partial class changscientistwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "scientistId",
                table: "WorkOfScientists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "scientistId",
                table: "WorkOfScientists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
