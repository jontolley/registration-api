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
    [Migration("20171204191626_AddedUserSubgroupCollectionTouserAndSubgroupEntities")]
    partial class AddedUserSubgroupCollectionTouserAndSubgroupEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Subgroups");
                });

            modelBuilder.Entity("Registration.API.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<string>("Name")
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

                    b.ToTable("UserSubgroup");
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
