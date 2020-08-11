using Microsoft.EntityFrameworkCore;

namespace StockPlatform.Data.Models
{
    public partial class StockComparisonDbContext : DbContext
    {
        public StockComparisonDbContext()
        {
        }

        public StockComparisonDbContext(DbContextOptions<StockComparisonDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockHistoricalData> StockHistoricalDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<StockHistoricalData>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.Stock)
                    .WithMany(p => p.StockHistoricalDatas)
                    .HasForeignKey(d => d.StockId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_StockHistoricalDatas_Stocks");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
