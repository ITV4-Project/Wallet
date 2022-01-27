﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebWallet.Models;

#nullable disable

namespace WebWallet.Migrations
{
    [DbContext(typeof(ContextDB))]
    [Migration("20220127203324_privateKey")]
    partial class privateKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("WebWallet.Models.TransactionModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDelegating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MerkleHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.Property<string>("privateKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TransactionModel");
                });

            modelBuilder.Entity("WebWallet.Models.TransactionRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<long>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Input")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<bool>("IsDelegating")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("MerkleHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Output")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("Signature")
                        .HasColumnType("BLOB");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TransactionRecord");
                });

            modelBuilder.Entity("WebWallet.Models.WalletModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Balance")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PassPhrase")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserFName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserLName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WalletName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}