﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Registration.API.Entities;
using System;

namespace Registration.API.Migrations
{
    [DbContext(typeof(RegistrationContext))]
    [Migration("20180724201254_AddIsWithMinorToAttendee")]
    partial class AddIsWithMinorToAttendee
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Registration.API.Entities.Accommodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Accommodations");
                });

            modelBuilder.Entity("Registration.API.Entities.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DaysAttending");

                    b.Property<bool>("Friday");

                    b.Property<bool>("Monday");

                    b.Property<bool>("Saturday");

                    b.Property<bool>("Thursday");

                    b.Property<bool>("Tuesday");

                    b.Property<bool>("Wednesday");

                    b.HasKey("Id");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("Registration.API.Entities.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttendanceId");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<int>("Fee");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("InsertedById");

                    b.Property<DateTime>("InsertedOn");

                    b.Property<bool>("IsAdult");

                    b.Property<bool>("IsWithMinor");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("ShirtSizeId");

                    b.Property<int>("SubgroupId");

                    b.Property<bool>("Triathlon");

                    b.Property<int?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("AttendanceId");

                    b.HasIndex("InsertedById");

                    b.HasIndex("ShirtSizeId");

                    b.HasIndex("SubgroupId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("Registration.API.Entities.AttendeeAccommodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccommodationId");

                    b.Property<int>("AttendeeId");

                    b.HasKey("Id");

                    b.HasIndex("AccommodationId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("AttendeeAccommodations");
                });

            modelBuilder.Entity("Registration.API.Entities.AttendeeMeritBadge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttendeeId");

                    b.Property<int>("MeritBadgeId");

                    b.Property<int>("SortOrder");

                    b.HasKey("Id");

                    b.HasIndex("AttendeeId");

                    b.HasIndex("MeritBadgeId");

                    b.ToTable("AttendeeMeritBadges");
                });

            modelBuilder.Entity("Registration.API.Entities.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20);

                    b.Property<DateTime>("ReceivedDateTime");

                    b.Property<string>("Stake")
                        .HasMaxLength(100);

                    b.Property<string>("Ward")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Registration.API.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Location")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Registration.API.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Registration.API.Entities.MeritBadge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MeritBadges");
                });

            modelBuilder.Entity("Registration.API.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Registration.API.Entities.ShirtSize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("SortOrder");

                    b.HasKey("Id");

                    b.HasIndex("Size")
                        .IsUnique();

                    b.ToTable("ShirtSizes");
                });

            modelBuilder.Entity("Registration.API.Entities.Subgroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<int>("GroupId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("PinNumber");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Subgroups");
                });

            modelBuilder.Entity("Registration.API.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Nickname")
                        .HasMaxLength(100);

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(1024);

                    b.Property<string>("SubscriberId")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("SubscriberId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Registration.API.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RoleId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Registration.API.Entities.UserSubgroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SubgroupId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("SubgroupId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSubgroups");
                });

            modelBuilder.Entity("Registration.API.Entities.Attendee", b =>
                {
                    b.HasOne("Registration.API.Entities.Attendance", "Attendance")
                        .WithMany("AttendanceAttendees")
                        .HasForeignKey("AttendanceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Registration.API.Entities.User", "InsertedBy")
                        .WithMany("InsertedAttendees")
                        .HasForeignKey("InsertedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Registration.API.Entities.ShirtSize", "ShirtSize")
                        .WithMany("ShirtSizeAttendees")
                        .HasForeignKey("ShirtSizeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Registration.API.Entities.Subgroup", "Subgroup")
                        .WithMany("Attendees")
                        .HasForeignKey("SubgroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Registration.API.Entities.User", "UpdatedBy")
                        .WithMany("UpdatedAttendees")
                        .HasForeignKey("UpdatedById")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Registration.API.Entities.AttendeeAccommodation", b =>
                {
                    b.HasOne("Registration.API.Entities.Accommodation", "Accommodation")
                        .WithMany("AttendeeAccommodations")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Registration.API.Entities.Attendee", "Attendee")
                        .WithMany("AttendeeAccommodations")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Registration.API.Entities.AttendeeMeritBadge", b =>
                {
                    b.HasOne("Registration.API.Entities.Attendee", "Attendee")
                        .WithMany("AttendeeMeritBadges")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Registration.API.Entities.MeritBadge", "MeritBadge")
                        .WithMany("AttendeeMeritBadges")
                        .HasForeignKey("MeritBadgeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Registration.API.Entities.Subgroup", b =>
                {
                    b.HasOne("Registration.API.Entities.Group", "Group")
                        .WithMany("Subgroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Registration.API.Entities.UserRole", b =>
                {
                    b.HasOne("Registration.API.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Registration.API.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Registration.API.Entities.UserSubgroup", b =>
                {
                    b.HasOne("Registration.API.Entities.Subgroup", "Subgroup")
                        .WithMany("UserSubgroups")
                        .HasForeignKey("SubgroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Registration.API.Entities.User", "User")
                        .WithMany("UserSubgroups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
