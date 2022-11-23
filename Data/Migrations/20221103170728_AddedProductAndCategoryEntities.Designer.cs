﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using fleaApi.Data;

#nullable disable

namespace fleaApi.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221103170728_AddedProductAndCategoryEntities")]
    partial class AddedProductAndCategoryEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("FleaApp_Api.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Point", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<float>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<int?>("MarketId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ShopId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MarketId");

                    b.HasIndex("ShopId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Market", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Desc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isOpen")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Markets");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Desc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("ShopId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isSoldOut")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Desc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<int>("MarketId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isOpen")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MarketId");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Point", b =>
                {
                    b.HasOne("FleaApp_Api.Entities.Market", null)
                        .WithMany("Points")
                        .HasForeignKey("MarketId");

                    b.HasOne("FleaApp_Api.Entities.Shop", null)
                        .WithMany("Points")
                        .HasForeignKey("ShopId");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Product", b =>
                {
                    b.HasOne("FleaApp_Api.Entities.Shop", "Shop")
                        .WithMany("Product")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FleaApp_Api.Entities.SubCategory", "SubCategory")
                        .WithMany("Products")
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Shop", b =>
                {
                    b.HasOne("FleaApp_Api.Entities.Market", "Market")
                        .WithMany("Shop")
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Market");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.SubCategory", b =>
                {
                    b.HasOne("FleaApp_Api.Entities.Category", "Category")
                        .WithMany("SubCategory")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Category", b =>
                {
                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Market", b =>
                {
                    b.Navigation("Points");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.Shop", b =>
                {
                    b.Navigation("Points");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("FleaApp_Api.Entities.SubCategory", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
