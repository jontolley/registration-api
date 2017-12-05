using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Registration.API.Migrations
{
    public partial class AddedAttendeeMeritBadgeAccommodationShirtSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeritBadges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeritBadges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShirtSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShirtSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAdult = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShirtSizeId = table.Column<int>(type: "int", nullable: false),
                    Triathlon = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendees_ShirtSizes_ShirtSizeId",
                        column: x => x.ShirtSizeId,
                        principalTable: "ShirtSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendeeAccommodations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccommodationId = table.Column<int>(type: "int", nullable: false),
                    AttendeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeeAccommodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendeeAccommodations_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendeeAccommodations_Attendees_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "Attendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendeeMeritBadges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttendeeId = table.Column<int>(type: "int", nullable: false),
                    MeritBadgeId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeeMeritBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendeeMeritBadges_Attendees_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "Attendees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendeeMeritBadges_MeritBadges_MeritBadgeId",
                        column: x => x.MeritBadgeId,
                        principalTable: "MeritBadges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_Name",
                table: "Accommodations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeAccommodations_AccommodationId",
                table: "AttendeeAccommodations",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeAccommodations_AttendeeId",
                table: "AttendeeAccommodations",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeMeritBadges_AttendeeId",
                table: "AttendeeMeritBadges",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeMeritBadges_MeritBadgeId",
                table: "AttendeeMeritBadges",
                column: "MeritBadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_ShirtSizeId",
                table: "Attendees",
                column: "ShirtSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_MeritBadges_Name",
                table: "MeritBadges",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShirtSizes_Size",
                table: "ShirtSizes",
                column: "Size",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendeeAccommodations");

            migrationBuilder.DropTable(
                name: "AttendeeMeritBadges");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Attendees");

            migrationBuilder.DropTable(
                name: "MeritBadges");

            migrationBuilder.DropTable(
                name: "ShirtSizes");
        }
    }
}
