﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Data;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250317085709_AddCart")]
    partial class AddCart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Server.Entity.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartId"));

                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CartId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Server.Entity.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartItemId"));

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("CartItemId");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItem");
                });

            modelBuilder.Entity("Server.Entity.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Raters")
                        .HasColumnType("int");

                    b.Property<decimal>("Rating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            Description = "S23 description",
                            ImageUrl = "s23.jpg",
                            IsActive = true,
                            Name = "Galaxy S23 256 GB",
                            Price = 35000m,
                            Raters = 0,
                            Rating = 0m,
                            Stock = 5000
                        },
                        new
                        {
                            ProductId = 2,
                            Description = "15 Pro description",
                            ImageUrl = "15pro.jpg",
                            IsActive = true,
                            Name = "IPhone 15 Pro 512 GB",
                            Price = 75000m,
                            Raters = 0,
                            Rating = 0m,
                            Stock = 2000
                        },
                        new
                        {
                            ProductId = 3,
                            Description = "Lego set description",
                            ImageUrl = "lego.jpg",
                            IsActive = true,
                            Name = "Star Wars Darth Vader Lego Set",
                            Price = 3000m,
                            Raters = 0,
                            Rating = 0m,
                            Stock = 4200
                        },
                        new
                        {
                            ProductId = 4,
                            Description = "G Pro X Superlight description",
                            ImageUrl = "gprox.jpg",
                            IsActive = false,
                            Name = "Logitech G Pro X Superlight",
                            Price = 5500m,
                            Raters = 0,
                            Rating = 0m,
                            Stock = 30
                        },
                        new
                        {
                            ProductId = 5,
                            Description = "Buds FE description",
                            ImageUrl = "budsfe.jpg",
                            IsActive = true,
                            Name = "Galaxy Buds FE",
                            Price = 2000m,
                            Raters = 0,
                            Rating = 0m,
                            Stock = 0
                        });
                });

            modelBuilder.Entity("Server.Entity.CartItem", b =>
                {
                    b.HasOne("Server.Entity.Cart", null)
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Entity.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Server.Entity.Cart", b =>
                {
                    b.Navigation("CartItems");
                });
#pragma warning restore 612, 618
        }
    }
}
