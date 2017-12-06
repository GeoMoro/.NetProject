using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Data.Persistance.Migrations
{
    public partial class UserMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    Group = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    RegistrationNumber = table.Column<string>(nullable: true),
                    Validated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
