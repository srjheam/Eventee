﻿// <auto-generated />
using System;
using Eventee.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Eventee.Api.Migrations.Eventee
{
    [DbContext(typeof(EventeeContext))]
    [Migration("20230922022453_DisableDbGenPkForGetTogether")]
    partial class DisableDbGenPkForGetTogether
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Eventee.Api.Models.GetTogether", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("HosterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ScheduleDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HosterId");

                    b.ToTable("GetTogethers");
                });

            modelBuilder.Entity("Eventee.Api.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GetTogetherUser", b =>
                {
                    b.Property<int>("SubscribedGetTogethersId")
                        .HasColumnType("int");

                    b.Property<string>("SubscribersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SubscribedGetTogethersId", "SubscribersId");

                    b.HasIndex("SubscribersId");

                    b.ToTable("GetTogetherUser");
                });

            modelBuilder.Entity("Eventee.Api.Models.GetTogether", b =>
                {
                    b.HasOne("Eventee.Api.Models.User", "Hoster")
                        .WithMany("HostedGetTogethers")
                        .HasForeignKey("HosterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Hoster");
                });

            modelBuilder.Entity("GetTogetherUser", b =>
                {
                    b.HasOne("Eventee.Api.Models.GetTogether", null)
                        .WithMany()
                        .HasForeignKey("SubscribedGetTogethersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eventee.Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("SubscribersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Eventee.Api.Models.User", b =>
                {
                    b.Navigation("HostedGetTogethers");
                });
#pragma warning restore 612, 618
        }
    }
}
