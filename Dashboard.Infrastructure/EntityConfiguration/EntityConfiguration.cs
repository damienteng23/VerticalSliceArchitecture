using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.EntityConfiguration
{
    public class CandleConfiguration : IEntityTypeConfiguration<Candle>
    {
        public void Configure(EntityTypeBuilder<Candle> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Symbol).IsRequired();
            builder.Property(p => p.Timestamp).IsRequired();
            builder.Property(p => p.Open).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.High).HasPrecision(18, 8).IsRequired(); 
            builder.Property(p => p.Low).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.Close).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.Volume).IsRequired();
            builder.Property(p => p.AdjustedClose).HasPrecision(18, 8).IsRequired();

            // Composite Index: (Symbol + Timestamp)
            // Convention naming: [Type] _ [Table] _ [Column1] _ [Column2]
            /* How to use
             * return await _context.Ticks
                    .Where(t => t.Symbol == symbol)               // Step 1: Jump to the Symbol section
                    .Where(t => t.Timestamp >= startTime)      // Step 2: Seek the Start Time
                    .Where(t => t.Timestamp <= endTime)       // Step 3: Stop at the End Time
                    .OrderBy(t => t.Timestamp)                         // Step 4: Already sorted by index!
                    .ToListAsync();
             */
            builder.HasIndex(p => new { p.Symbol, p.Timestamp })
                   .HasDatabaseName("IX_Tick_Symbol_Timestamp");

            // Single timestamp index for global queries across all symbols
            // Convention naming: [Type] _ [Table] _ [Column1] 
            /* How to use
             * return await _context.Ticks
                    .OrderByDescending(t => t.Timestamp)     // Step 1: Use the Index to find the newest
                    .Take(limit)                                                   // Step 2: Grab the top 50
                    .ToListAsync();
             */
            builder.HasIndex(p => p.Timestamp)
                   .HasDatabaseName("IX_Tick_Timestamp");
        }
    }
}
