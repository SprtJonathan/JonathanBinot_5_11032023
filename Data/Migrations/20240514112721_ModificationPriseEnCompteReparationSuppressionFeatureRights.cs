using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModificationPriseEnCompteReparationSuppressionFeatureRights : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reparations_Vehicles_VehicleId",
                table: "Reparations");

            migrationBuilder.DropTable(
                name: "AccountRights");

            migrationBuilder.DropIndex(
                name: "IX_Reparations_VehicleId",
                table: "Reparations");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FeatureRights",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "CoutsReparation",
                table: "Vehicles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reparations",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoutsReparation",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Reparations",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FeatureRights",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountRights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeatureRight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRights_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reparations_VehicleId",
                table: "Reparations",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRights_AccountId",
                table: "AccountRights",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reparations_Vehicles_VehicleId",
                table: "Reparations",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
