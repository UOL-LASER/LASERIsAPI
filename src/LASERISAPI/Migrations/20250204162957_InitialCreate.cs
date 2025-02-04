using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LASERISAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    manufacturerName = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    serialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    orderCode = table.Column<string>(type: "TEXT", nullable: true),
                    itemType = table.Column<string>(type: "TEXT", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    signedOutTo = table.Column<string>(type: "TEXT", nullable: true),
                    signedOutToId = table.Column<int>(type: "INTEGER", nullable: true),
                    signedOutDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entries");
        }
    }
}
