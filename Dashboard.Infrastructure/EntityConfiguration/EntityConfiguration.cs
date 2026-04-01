using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.EntityConfiguration
{
    public class DailyTickerSummaryConfiguration : IEntityTypeConfiguration<DailyTickerSummary>
    {
        public void Configure(EntityTypeBuilder<DailyTickerSummary> builder)
        {
            builder.HasKey(p => new { p.Symbol, p.From });

            builder.Property(p => p.Symbol).IsRequired();
            builder.Property(p => p.From).IsRequired();
            builder.Property(p => p.Open).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.High).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.Low).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.Close).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.Volume).IsRequired();
            builder.Property(p => p.PreMarket).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.AfterHours).HasPrecision(18, 8).IsRequired();
            builder.Property(p => p.Status).IsRequired();

            // Composite Primary Key: (Symbol + From)
            // Convention naming: [Type] _ [Table] _ [Column1] _ [Column2]
            /* How to use
             * return await _context.DailyTickerSummaries
                    .Where(d => d.Symbol == symbol)           // Step 1: Jump to the Symbol section
                    .Where(d => d.From >= startDate)          // Step 2: Seek the Start Date
                    .Where(d => d.From <= endDate)            // Step 3: Stop at the End Date
                    .OrderBy(d => d.From)                     // Step 4: Already sorted by composite key!
                    .ToListAsync();
             */
            // Primary key is composite, no need for additional index

            // Single date index for global queries across all symbols
            // Convention naming: [Type] _ [Table] _ [Column1]
            /* How to use
             * return await _context.DailyTickerSummaries
                    .OrderByDescending(d => d.From)           // Step 1: Use the Index to find the newest
                    .Take(limit)                              // Step 2: Grab the top 50
                    .ToListAsync();
             */
            builder.HasIndex(p => p.From)
                   .HasDatabaseName("IX_DailyTickerSummary_From");
        }
    }
}