using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Registration.API.Migrations
{
    public partial class AddedSubgroupForeignKeyToAttendeeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubgroupId",
                table: "Attendees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_SubgroupId",
                table: "Attendees",
                column: "SubgroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Subgroups_SubgroupId",
                table: "Attendees",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Subgroups_SubgroupId",
                table: "Attendees");

            migrationBuilder.DropIndex(
                name: "IX_Attendees_SubgroupId",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "SubgroupId",
                table: "Attendees");
        }
    }
}
