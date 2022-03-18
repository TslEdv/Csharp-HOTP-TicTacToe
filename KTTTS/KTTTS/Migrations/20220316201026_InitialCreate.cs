using Microsoft.EntityFrameworkCore.Migrations;

namespace KTTTS.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    P1Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    P1Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    P1Counter = table.Column<int>(type: "int", nullable: false),
                    P2Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    P2Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    P2Counter = table.Column<int>(type: "int", nullable: false),
                    GameState = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
