using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_zawodnicy_zimowi.DATA.Migrations
{
    /// <inheritdoc />
    public partial class DodanieUzytkownikow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrudnoscTrasy",
                table: "Wyniki",
                newName: "TrudnoscHistoryczna");

            migrationBuilder.RenameColumn(
                name: "PunktyBazowe",
                table: "Wyniki",
                newName: "PunktyBazoweHistoryczne");

            migrationBuilder.RenameColumn(
                name: "NazwaZawodow",
                table: "Wyniki",
                newName: "NazwaHistoryczna");

            migrationBuilder.AddColumn<Guid>(
                name: "RodzajZawodowId",
                table: "Wyniki",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RodzajeZawodow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trudnosc = table.Column<int>(type: "int", nullable: false),
                    PunktyBazowe = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RodzajeZawodow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasloHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wyniki_RodzajZawodowId",
                table: "Wyniki",
                column: "RodzajZawodowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wyniki_RodzajeZawodow_RodzajZawodowId",
                table: "Wyniki",
                column: "RodzajZawodowId",
                principalTable: "RodzajeZawodow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wyniki_RodzajeZawodow_RodzajZawodowId",
                table: "Wyniki");

            migrationBuilder.DropTable(
                name: "RodzajeZawodow");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");

            migrationBuilder.DropIndex(
                name: "IX_Wyniki_RodzajZawodowId",
                table: "Wyniki");

            migrationBuilder.DropColumn(
                name: "RodzajZawodowId",
                table: "Wyniki");

            migrationBuilder.RenameColumn(
                name: "TrudnoscHistoryczna",
                table: "Wyniki",
                newName: "TrudnoscTrasy");

            migrationBuilder.RenameColumn(
                name: "PunktyBazoweHistoryczne",
                table: "Wyniki",
                newName: "PunktyBazowe");

            migrationBuilder.RenameColumn(
                name: "NazwaHistoryczna",
                table: "Wyniki",
                newName: "NazwaZawodow");
        }
    }
}
