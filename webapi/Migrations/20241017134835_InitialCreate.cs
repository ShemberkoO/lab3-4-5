using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:transport_type", "car,truck,motorcycle,scooter,pedestrian");

            migrationBuilder.CreateTable(
                name: "accidents",
                columns: table => new
                {
                    accident_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    number_of_injured = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "0"),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("accidents_pkey", x => x.accident_id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    pasport_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    registration_address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("person_pkey", x => x.pasport_id);
                });

            migrationBuilder.CreateTable(
                name: "transport",
                columns: table => new
                {
                    transport_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    registration_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    year_of_manufacture = table.Column<int>(type: "integer", nullable: true),
                    owner_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transport_pkey", x => x.transport_id);
                    table.ForeignKey(
                        name: "transport_owner_id_fkey",
                        column: x => x.owner_id,
                        principalTable: "person",
                        principalColumn: "pasport_id");
                });

            migrationBuilder.CreateTable(
                name: "victims",
                columns: table => new
                {
                    victim_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pasport_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'неушкоджений'::character varying"),
                    accident_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("victims_pkey", x => x.victim_id);
                    table.ForeignKey(
                        name: "victims_accident_id_fkey",
                        column: x => x.accident_id,
                        principalTable: "accidents",
                        principalColumn: "accident_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "victims_pasport_id_fkey",
                        column: x => x.pasport_id,
                        principalTable: "person",
                        principalColumn: "pasport_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "occupant_transport",
                columns: table => new
                {
                    victim_id = table.Column<int>(type: "integer", nullable: false),
                    transport_id = table.Column<int>(type: "integer", nullable: false),
                    driver_license_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("occupant_transport_pkey", x => x.victim_id);
                    table.ForeignKey(
                        name: "occupant_transport_transport_id_fkey",
                        column: x => x.transport_id,
                        principalTable: "transport",
                        principalColumn: "transport_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "occupant_transport_victim_id_fkey",
                        column: x => x.victim_id,
                        principalTable: "victims",
                        principalColumn: "victim_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "violations",
                columns: table => new
                {
                    violation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    article = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    victim_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("violations_pkey", x => x.violation_id);
                    table.ForeignKey(
                        name: "violations_victim_id_fkey",
                        column: x => x.victim_id,
                        principalTable: "victims",
                        principalColumn: "victim_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_occupant_transport_transport_id",
                table: "occupant_transport",
                column: "transport_id");

            migrationBuilder.CreateIndex(
                name: "IX_transport_owner_id",
                table: "transport",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_victims_accident_id",
                table: "victims",
                column: "accident_id");

            migrationBuilder.CreateIndex(
                name: "IX_victims_pasport_id",
                table: "victims",
                column: "pasport_id");

            migrationBuilder.CreateIndex(
                name: "IX_violations_victim_id",
                table: "violations",
                column: "victim_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "occupant_transport");

            migrationBuilder.DropTable(
                name: "violations");

            migrationBuilder.DropTable(
                name: "transport");

            migrationBuilder.DropTable(
                name: "victims");

            migrationBuilder.DropTable(
                name: "accidents");

            migrationBuilder.DropTable(
                name: "person");
        }
    }
}
