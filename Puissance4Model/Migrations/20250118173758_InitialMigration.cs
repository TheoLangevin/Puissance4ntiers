using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puissance4Model.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Grids_GridId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Cells");

            migrationBuilder.DropIndex(
                name: "IX_Games_GridId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grids",
                table: "Grids");

            migrationBuilder.DropColumn(
                name: "GridId",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Grids",
                newName: "Grid");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Grid",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grid",
                table: "Grid",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cell",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Row = table.Column<int>(type: "INTEGER", nullable: false),
                    Column = table.Column<int>(type: "INTEGER", nullable: false),
                    TokenId = table.Column<int>(type: "INTEGER", nullable: true),
                    GridId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cell_Grid_GridId",
                        column: x => x.GridId,
                        principalTable: "Grid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cell_Token_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Token",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_Login",
                table: "Players",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grid_GameId",
                table: "Grid",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cell_GridId",
                table: "Cell",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_Cell_TokenId",
                table: "Cell",
                column: "TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grid_Games_GameId",
                table: "Grid",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grid_Games_GameId",
                table: "Grid");

            migrationBuilder.DropTable(
                name: "Cell");

            migrationBuilder.DropIndex(
                name: "IX_Players_Login",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grid",
                table: "Grid");

            migrationBuilder.DropIndex(
                name: "IX_Grid_GameId",
                table: "Grid");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Grid");

            migrationBuilder.RenameTable(
                name: "Grid",
                newName: "Grids");

            migrationBuilder.AddColumn<int>(
                name: "GridId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grids",
                table: "Grids",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TokenId = table.Column<int>(type: "INTEGER", nullable: false),
                    Column = table.Column<int>(type: "INTEGER", nullable: false),
                    GridId = table.Column<int>(type: "INTEGER", nullable: true),
                    Row = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cells_Grids_GridId",
                        column: x => x.GridId,
                        principalTable: "Grids",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cells_Token_TokenId",
                        column: x => x.TokenId,
                        principalTable: "Token",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GridId",
                table: "Games",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_Cells_GridId",
                table: "Cells",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_Cells_TokenId",
                table: "Cells",
                column: "TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Grids_GridId",
                table: "Games",
                column: "GridId",
                principalTable: "Grids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
