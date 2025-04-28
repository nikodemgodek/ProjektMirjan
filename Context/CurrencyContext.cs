using Microsoft.EntityFrameworkCore;
using ProjektMirjan.Model;

namespace ProjektMirjan.Context
{
    public class CurrencyContext : DbContext
    {
        public DbSet<CurrencyRateTable> CurrencyRateTables { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrencyRateTable>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Table, e.No }).IsUnique();
            });

            modelBuilder.Entity<CurrencyRate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CurrencyRateTable)
                      .WithMany(t => t.CurrencyRates)
                      .HasForeignKey(e => e.CurrencyRateTableId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
