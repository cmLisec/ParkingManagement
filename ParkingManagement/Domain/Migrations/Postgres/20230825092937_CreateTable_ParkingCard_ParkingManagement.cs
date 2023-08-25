using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ParkingManagement.Domain.Migrations.Postgres
{
    public partial class CreateTable_ParkingCard_ParkingManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardDetailsId",
                table: "ParkingCard",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "ParkingCard",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CardDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardName = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingCard_CardDetailsId",
                table: "ParkingCard",
                column: "CardDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingCard_CardDetails_CardDetailsId",
                table: "ParkingCard",
                column: "CardDetailsId",
                principalTable: "CardDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingCard_CardDetails_CardDetailsId",
                table: "ParkingCard");

            migrationBuilder.DropTable(
                name: "CardDetails");

            migrationBuilder.DropIndex(
                name: "IX_ParkingCard_CardDetailsId",
                table: "ParkingCard");

            migrationBuilder.DropColumn(
                name: "CardDetailsId",
                table: "ParkingCard");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "ParkingCard");
        }
    }
}
