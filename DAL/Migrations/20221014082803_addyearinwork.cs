using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parser1.Migrations
{
    public partial class addyearinwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "WorkOfScientists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "WorkOfScientists");
        }
    }
}
