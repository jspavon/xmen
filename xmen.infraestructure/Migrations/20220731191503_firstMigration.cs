using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace xmen.infraestructure.Migrations
{
    public partial class firstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "TBL_Mutant",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    And = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Humans = table.Column<int>(type: "int", nullable: false),
                    Mutants = table.Column<int>(type: "int", nullable: false),
                    Radio = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreationUser = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModificationUser = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_Mutant", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_Mutant",
                schema: "dbo");
        }
    }
}
