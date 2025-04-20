using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Context.Migrations
{
    public partial class StringEntityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cards_id",
                table: "cards");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "cards",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldDefaultValueSql: "LPAD(CAST(FLOOR(RAND() * 1000000000000000) AS CHAR), 15, '0')");

            migrationBuilder.CreateIndex(
                name: "IX_cards_id",
                table: "cards",
                column: "id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cards_id",
                table: "cards");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "cards",
                type: "varchar(255)",
                nullable: false,
                defaultValueSql: "LPAD(CAST(FLOOR(RAND() * 1000000000000000) AS CHAR), 15, '0')",
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.CreateIndex(
                name: "IX_cards_id",
                table: "cards",
                column: "id");
        }
    }
}
