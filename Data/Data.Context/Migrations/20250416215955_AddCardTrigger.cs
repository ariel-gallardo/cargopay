using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Context.Migrations
{
    public partial class AddCardTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER before_insert_cards
                BEFORE INSERT ON cards
                FOR EACH ROW
                BEGIN
                    IF NEW.id IS NULL THEN
                        SET NEW.id = LPAD(CAST(FLOOR(RAND() * 1000000000000000) AS CHAR), 15, '0');
                    END IF;
                    SET @last_id = NEW.id;
                    SELECT @last_id;    
                END;
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS before_insert_cards;");
        }
    }
}
