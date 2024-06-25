using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebApiTutorial.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Sales = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
        } //HOW YOU MAKE CHANGES TO YOUR DB - MOVES DB FORWARD

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) //REVERSES WHAT WAS DONE LIKE A ROLLBACK
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
