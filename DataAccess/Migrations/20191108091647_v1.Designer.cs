﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20191108091647_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0-preview1.19506.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataInterface.Aisle", b =>
                {
                    b.Property<int>("AisleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AisleNumber")
                        .HasColumnType("int");

                    b.HasKey("AisleID");

                    b.ToTable("Aisles");
                });

            modelBuilder.Entity("DataInterface.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BookAuthor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BookCondition")
                        .HasColumnType("int");

                    b.Property<int>("BookPrice")
                        .HasColumnType("int");

                    b.Property<string>("BookTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLoaned")
                        .HasColumnType("bit");

                    b.Property<string>("IsbnNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PurchaseYear")
                        .HasColumnType("int");

                    b.Property<int>("ShelfID")
                        .HasColumnType("int");

                    b.HasKey("BookID");

                    b.HasIndex("ShelfID");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("DataInterface.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BirthDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerAdress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Debt")
                        .HasColumnType("int");

                    b.Property<int?>("GuardianCustomerID")
                        .HasColumnType("int");

                    b.HasKey("CustomerID");

                    b.HasIndex("GuardianCustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DataInterface.Loan", b =>
                {
                    b.Property<int>("LoanID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookID")
                        .HasColumnType("int");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LoanDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.HasKey("LoanID");

                    b.HasIndex("BookID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("DataInterface.Shelf", b =>
                {
                    b.Property<int>("ShelfID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AisleID")
                        .HasColumnType("int");

                    b.Property<int>("AisleNumber")
                        .HasColumnType("int");

                    b.Property<int>("ShelfNumber")
                        .HasColumnType("int");

                    b.HasKey("ShelfID");

                    b.HasIndex("AisleID");

                    b.ToTable("Shelfs");
                });

            modelBuilder.Entity("DataInterface.Book", b =>
                {
                    b.HasOne("DataInterface.Shelf", "Shelf")
                        .WithMany("Book")
                        .HasForeignKey("ShelfID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataInterface.Customer", b =>
                {
                    b.HasOne("DataInterface.Customer", "Guardian")
                        .WithMany()
                        .HasForeignKey("GuardianCustomerID");
                });

            modelBuilder.Entity("DataInterface.Loan", b =>
                {
                    b.HasOne("DataInterface.Book", "Book")
                        .WithMany("Loan")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataInterface.Customer", "Customer")
                        .WithMany("Loan")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DataInterface.Shelf", b =>
                {
                    b.HasOne("DataInterface.Aisle", "Aisle")
                        .WithMany("Shelf")
                        .HasForeignKey("AisleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
