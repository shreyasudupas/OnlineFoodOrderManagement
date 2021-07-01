﻿// <auto-generated />
using System;
using Identity.MicroService.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.MicroService.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20210630145639_First-Migration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Identity.MicroService.Data.City", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CityNames")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<long>("StateId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("Identity.MicroService.Data.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActionMethod")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ControllerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("Identity.MicroService.Data.State", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("StateNames")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("State");
                });

            modelBuilder.Entity("Identity.MicroService.Data.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("CartAmount")
                        .HasColumnType("float");

                    b.Property<long?>("CityId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PictureLocation")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<long>("Points")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StateId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("RoleId");

                    b.HasIndex("StateId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Identity.MicroService.Data.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Rolename")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Identity.MicroService.Data.City", b =>
                {
                    b.HasOne("Identity.MicroService.Data.State", "State")
                        .WithMany("Cities")
                        .HasForeignKey("StateId")
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("Identity.MicroService.Data.User", b =>
                {
                    b.HasOne("Identity.MicroService.Data.City", "City")
                        .WithMany("Users")
                        .HasForeignKey("CityId");

                    b.HasOne("Identity.MicroService.Data.UserRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Identity.MicroService.Data.State", "State")
                        .WithMany("Users")
                        .HasForeignKey("StateId");

                    b.Navigation("City");

                    b.Navigation("Role");

                    b.Navigation("State");
                });

            modelBuilder.Entity("Identity.MicroService.Data.City", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Identity.MicroService.Data.State", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Identity.MicroService.Data.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
