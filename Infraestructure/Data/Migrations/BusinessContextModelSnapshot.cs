﻿// <auto-generated />
using System;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infraestructure.Data.Migrations
{
    [DbContext(typeof(BusinessContext))]
    partial class BusinessContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Core.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Temperature")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Core.Entities.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("stores", (string)null);
                });

            modelBuilder.Entity("Core.Entities.StoreProducts", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)");

                    b.HasKey("ProductId", "StoreId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreProducts");
                });

            modelBuilder.Entity("Core.Entities.StoreProducts", b =>
                {
                    b.HasOne("Core.Entities.Product", "Product")
                        .WithMany("StoreProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Store", "Store")
                        .WithMany("StoreProducts")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Core.Entities.Product", b =>
                {
                    b.Navigation("StoreProducts");
                });

            modelBuilder.Entity("Core.Entities.Store", b =>
                {
                    b.Navigation("StoreProducts");
                });
#pragma warning restore 612, 618
        }
    }
}