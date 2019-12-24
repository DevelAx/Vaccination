using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Vaccination.EF.Migrations
{
    public partial class CreateInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IntId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Patronymic = table.Column<string>(nullable: true),
                    NormalizedFirstName = table.Column<string>(nullable: false),
                    NormalizedLastName = table.Column<string>(nullable: false),
                    NormalizedPatronymic = table.Column<string>(nullable: true),
                    NormalizedFullName = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<string>(nullable: false),
                    InsuranceNumber = table.Column<string>(maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IntId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inoculations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IntId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: true),
                    PatientId = table.Column<Guid>(nullable: false),
                    VaccineId = table.Column<Guid>(nullable: false),
                    HasConsent = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inoculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inoculations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inoculations_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inoculations_IntId",
                table: "Inoculations",
                column: "IntId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inoculations_PatientId",
                table: "Inoculations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Inoculations_VaccineId",
                table: "Inoculations",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceNumber",
                table: "Patients",
                column: "InsuranceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IntId",
                table: "Patients",
                column: "IntId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vaccines_IntId",
                table: "Vaccines",
                column: "IntId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inoculations");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Vaccines");
        }
    }
}
