using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Registration.API.Migrations
{
    public partial class AddedAttendanceEntityAndFKInAttendee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_ShirtSizes_ShirtSizeId",
                table: "Attendees");

            migrationBuilder.AddColumn<int>(
                name: "AttendanceId",
                table: "Attendees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Friday = table.Column<bool>(type: "bit", nullable: false),
                    Monday = table.Column<bool>(type: "bit", nullable: false),
                    Saturday = table.Column<bool>(type: "bit", nullable: false),
                    Thursday = table.Column<bool>(type: "bit", nullable: false),
                    Tuesday = table.Column<bool>(type: "bit", nullable: false),
                    Wednesday = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_AttendanceId",
                table: "Attendees",
                column: "AttendanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Attendance_AttendanceId",
                table: "Attendees",
                column: "AttendanceId",
                principalTable: "Attendance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_ShirtSizes_ShirtSizeId",
                table: "Attendees",
                column: "ShirtSizeId",
                principalTable: "ShirtSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Attendance_AttendanceId",
                table: "Attendees");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_ShirtSizes_ShirtSizeId",
                table: "Attendees");

            migrationBuilder.DropTable(
                name: "Attendance");

            migrationBuilder.DropIndex(
                name: "IX_Attendees_AttendanceId",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "Attendees");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_ShirtSizes_ShirtSizeId",
                table: "Attendees",
                column: "ShirtSizeId",
                principalTable: "ShirtSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
