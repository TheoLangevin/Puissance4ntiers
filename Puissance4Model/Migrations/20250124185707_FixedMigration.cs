using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puissance4Model.Migrations
{
    /// <inheritdoc />
    public partial class FixedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cell_Grid_GridId",
                table: "Cell");

            migrationBuilder.DropForeignKey(
                name: "FK_Cell_Token_TokenId",
                table: "Cell");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cell",
                table: "Cell");

            migrationBuilder.DropIndex(
                name: "IX_Cell_TokenId",
                table: "Cell");

            migrationBuilder.DropColumn(
                name: "TokenId",
                table: "Cell");

            migrationBuilder.RenameTable(
                name: "Cell",
                newName: "Cells");

            migrationBuilder.RenameIndex(
                name: "IX_Cell_GridId",
                table: "Cells",
                newName: "IX_Cells_GridId");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Cells",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cells",
                table: "Cells",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cells_Grid_GridId",
                table: "Cells",
                column: "GridId",
                principalTable: "Grid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cells_Grid_GridId",
                table: "Cells");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cells",
                table: "Cells");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Cells");

            migrationBuilder.RenameTable(
                name: "Cells",
                newName: "Cell");

            migrationBuilder.RenameIndex(
                name: "IX_Cells_GridId",
                table: "Cell",
                newName: "IX_Cell_GridId");

            migrationBuilder.AddColumn<int>(
                name: "TokenId",
                table: "Cell",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cell",
                table: "Cell",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Color = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cell_TokenId",
                table: "Cell",
                column: "TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cell_Grid_GridId",
                table: "Cell",
                column: "GridId",
                principalTable: "Grid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cell_Token_TokenId",
                table: "Cell",
                column: "TokenId",
                principalTable: "Token",
                principalColumn: "Id");
        }
    }
}
