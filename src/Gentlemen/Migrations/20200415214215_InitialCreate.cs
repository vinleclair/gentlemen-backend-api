﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gentlemen.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barbers",
                columns: table => new
                {
                    BarberId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbers", x => x.BarberId);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientName = table.Column<string>(nullable: true),
                    ClientEmail = table.Column<string>(nullable: true),
                    ScheduledDate = table.Column<DateTime>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    BarberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointments_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "BarberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Barbers",
                columns: new[] { "BarberId", "ImagePath", "Name" },
                values: new object[] { 1, "matthew.png", "Matthew" });

            migrationBuilder.InsertData(
                table: "Barbers",
                columns: new[] { "BarberId", "ImagePath", "Name" },
                values: new object[] { 2, "fredrick.png", "Fredrick" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "Duration", "Name", "Price" },
                values: new object[] { 1, 30, "Haircut", 26 });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "Duration", "Name", "Price" },
                values: new object[] { 2, 30, "Shave", 20 });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_BarberId",
                table: "Appointments",
                column: "BarberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Barbers");
        }
    }
}
