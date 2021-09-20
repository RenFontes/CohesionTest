using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CohesionTest.Db.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    buildingCode = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    currentStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    createdBy = table.Column<string>(type: "TEXT", nullable: false),
                    createdDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "TEXT", nullable: false),
                    lastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequests");
        }
    }
}
