﻿// <auto-generated />
using System;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240329135554_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Domain.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OrderCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1000);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderCode"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Domain.Entities.OrderProduct", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("Domain.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("QrCode")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("Domain.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Domain.Entities.Order", b =>
                {
                    b.HasOne("Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Domain.Entities.OrderProduct", b =>
                {
                    b.HasOne("Domain.Entities.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Entities.Payment", b =>
                {
                    b.HasOne("Domain.Entities.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("Domain.Entities.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Domain.Entities.Order", b =>
                {
                    b.Navigation("Payment")
                        .IsRequired();

                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
