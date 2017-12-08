using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Registration.API.Migrations
{
    public partial class FixCascadeDeleteKFIssueWithSubgroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Subgroups_SubgroupId",
                table: "Attendees");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Subgroups_SubgroupId",
                table: "Attendees",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Subgroups_SubgroupId",
                table: "Attendees");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Subgroups_SubgroupId",
                table: "Attendees",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
