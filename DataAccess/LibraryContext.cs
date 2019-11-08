using DataInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class LibraryContext : DbContext
    {
        private const string connectionString = "Server=LAPTOP-FF92I8LI;Database=Library;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var key = modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetForeignKeys().Where(
                    fk => fk.DeclaringEntityType.ClrType.Name == "Loan" &&
                            fk.DependentToPrincipal.ClrType.Name == "Customer")).First();
            key.DeleteBehavior = DeleteBehavior.Restrict;
        }

        public DbSet<Aisle> Aisles { get; set; }

        public DbSet<Shelf> Shelfs { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Loan> Loans { get; set; }

    }
}
