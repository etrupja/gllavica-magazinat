﻿// <auto-generated />
using GllavicaInventari.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace GllavicaInventari.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180523091827_TransferAdded")]
    partial class TransferAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GllavicaInventari.Models.Entry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime>("DateEntry");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LoggedInUserId");

                    b.Property<double>("Price");

                    b.Property<int>("ProductId");

                    b.Property<int>("SupplierId");

                    b.Property<double>("TotalValue");

                    b.Property<int>("WareHouseId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("WareHouseId");

                    b.ToTable("Entires");
                });

            modelBuilder.Entity("GllavicaInventari.Models.Exit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime>("DateExit");

                    b.Property<string>("LoggedInUserId");

                    b.Property<double>("Price");

                    b.Property<int>("ProductId");

                    b.Property<int>("SupplierId");

                    b.Property<double>("TotalValue");

                    b.Property<int>("WareHouseId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("WareHouseId");

                    b.ToTable("Exits");
                });

            modelBuilder.Entity("GllavicaInventari.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LoggedInUserId");

                    b.Property<string>("Title");

                    b.Property<string>("Unit");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("GllavicaInventari.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Contact");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LoggedInUserId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("GllavicaInventari.Models.Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<DateTime>("DateTranfer");

                    b.Property<int>("FromWareHouseId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LoggedInUserId");

                    b.Property<double>("Price");

                    b.Property<int>("ProductId");

                    b.Property<int>("ToWareHouseId");

                    b.Property<double>("TotalValue");

                    b.Property<string>("TransferCode")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ToWareHouseId");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("GllavicaInventari.Models.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("GllavicaInventari.Models.Entry", b =>
                {
                    b.HasOne("GllavicaInventari.Models.Product", "Product")
                        .WithMany("Entries")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GllavicaInventari.Models.Supplier", "Supplier")
                        .WithMany("Entries")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GllavicaInventari.Models.Warehouse", "WareHouse")
                        .WithMany("Entries")
                        .HasForeignKey("WareHouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GllavicaInventari.Models.Exit", b =>
                {
                    b.HasOne("GllavicaInventari.Models.Product", "Product")
                        .WithMany("Exits")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GllavicaInventari.Models.Supplier", "Supplier")
                        .WithMany("Exits")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GllavicaInventari.Models.Warehouse", "WareHouse")
                        .WithMany("Exits")
                        .HasForeignKey("WareHouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GllavicaInventari.Models.Transfer", b =>
                {
                    b.HasOne("GllavicaInventari.Models.Product", "Product")
                        .WithMany("Transfers")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GllavicaInventari.Models.Warehouse", "WareHouse")
                        .WithMany("Transfers")
                        .HasForeignKey("ToWareHouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
