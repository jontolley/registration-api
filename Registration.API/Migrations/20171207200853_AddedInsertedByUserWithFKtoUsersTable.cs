using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Registration.API.Migrations
{
    public partial class AddedInsertedByUserWithFKtoUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InsertedById",
                table: "Attendees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedOn",
                table: "Attendees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Attendees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Attendees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_InsertedById",
                table: "Attendees",
                column: "InsertedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_UpdatedById",
                table: "Attendees",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Users_InsertedById",
                table: "Attendees",
                column: "InsertedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendees_Users_UpdatedById",
                table: "Attendees",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Users_InsertedById",
                table: "Attendees");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendees_Users_UpdatedById",
                table: "Attendees");

            migrationBuilder.DropIndex(
                name: "IX_Attendees_InsertedById",
                table: "Attendees");

            migrationBuilder.DropIndex(
                name: "IX_Attendees_UpdatedById",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "InsertedById",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "InsertedOn",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Attendees");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Attendees");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
