using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.EntityCore.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(e => e.FirstName).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.LastName).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(250).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(250).IsUnicode(false);
                entity.HasIndex(e => e.Email).IsUnique();

            });

            modelBuilder.Entity<ExchangeRate>(entity =>
            {
                entity.ToTable("ExchangeRates");
                entity
                .HasOne(a => a.User)
                .WithMany(a => a.ExchangeRates)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.CurrencyCodeFrom).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.CurrencyCodeTo).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Amount).HasPrecision(38,18);
                entity.Property(e => e.Rate).HasPrecision(38,18);
                entity.Property(e => e.Result).HasPrecision(38,18);
            });

        }
    }
}
