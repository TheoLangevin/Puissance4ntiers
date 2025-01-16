using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puissance4Model.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureGameModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cells_Grids_GridId",
                table: "Cells");

            migrationBuilder.DropForeignKey(
                name: "FK_Cells_Token_TokenId",
                table: "Cells");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Grids_GridId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GridId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grids",
                table: "Grids");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cells",
                table: "Cells");

            migrationBuilder.DropIndex(
                name: "IX_Cells_GridId",
                table: "Cells");

            migrationBuilder.DropColumn(
                name: "GridId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GridId",
                table: "Cells");

            migrationBuilder.RenameTable(
                name: "Grids",
                newName: "Grid");

            migrationBuilder.RenameTable(
                name: "Cells",
                newName: "Cell");

            migrationBuilder.RenameIndex(
                name: "IX_Cells_TokenId",
                table: "Cell",
                newName: "IX_Cell_TokenId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cell",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grid",
                table: "Grid",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cell",
                table: "Cell",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Login",
                table: "Players",
                column: "Login",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cell_Grid_Id",
                table: "Cell",
                column: "Id",
                principalTable: "Grid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cell_Token_TokenId",
                table: "Cell",
                column: "TokenId",
                principalTable: "Token",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Grid_Id",
                table: "Games",
                column: "Id",
                principalTable: "Grid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cell_Grid_Id",
                table: "Cell");

            migrationBuilder.DropForeignKey(
                name: "FK_Cell_Token_TokenId",
                table: "Cell");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Grid_Id",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Players_Login",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grid",
                table: "Grid");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cell",
                table: "Cell");

            migrationBuilder.RenameTable(
                name: "Grid",
                newName: "Grids");

            migrationBuilder.RenameTable(
                name: "Cell",
                newName: "Cells");

            migrationBuilder.RenameIndex(
                name: "IX_Cell_TokenId",
                table: "Cells",
                newName: "IX_Cells_TokenId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "GridId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cells",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "GridId",
                table: "Cells",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grids",
                table: "Grids",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cells",
                table: "Cells",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GridId",
                table: "Games",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_Cells_GridId",
                table: "Cells",
                column: "GridId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cells_Grids_GridId",
                table: "Cells",
                column: "GridId",
                principalTable: "Grids",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cells_Token_TokenId",
                table: "Cells",
                column: "TokenId",
                principalTable: "Token",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
