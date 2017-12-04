using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Registration.API.Migrations
{
    public partial class AddedPinNumberToSubgroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubgroup_Subgroups_SubgroupId",
                table: "UserSubgroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubgroup_Users_UserId",
                table: "UserSubgroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubgroup",
                table: "UserSubgroup");

            migrationBuilder.RenameTable(
                name: "UserSubgroup",
                newName: "UserSubgroups");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubgroup_UserId",
                table: "UserSubgroups",
                newName: "IX_UserSubgroups_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubgroup_SubgroupId",
                table: "UserSubgroups",
                newName: "IX_UserSubgroups_SubgroupId");

            migrationBuilder.AddColumn<int>(
                name: "PinNumber",
                table: "Subgroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubgroups",
                table: "UserSubgroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubgroups_Subgroups_SubgroupId",
                table: "UserSubgroups",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubgroups_Users_UserId",
                table: "UserSubgroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubgroups_Subgroups_SubgroupId",
                table: "UserSubgroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubgroups_Users_UserId",
                table: "UserSubgroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubgroups",
                table: "UserSubgroups");

            migrationBuilder.DropColumn(
                name: "PinNumber",
                table: "Subgroups");

            migrationBuilder.RenameTable(
                name: "UserSubgroups",
                newName: "UserSubgroup");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubgroups_UserId",
                table: "UserSubgroup",
                newName: "IX_UserSubgroup_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubgroups_SubgroupId",
                table: "UserSubgroup",
                newName: "IX_UserSubgroup_SubgroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubgroup",
                table: "UserSubgroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubgroup_Subgroups_SubgroupId",
                table: "UserSubgroup",
                column: "SubgroupId",
                principalTable: "Subgroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubgroup_Users_UserId",
                table: "UserSubgroup",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
