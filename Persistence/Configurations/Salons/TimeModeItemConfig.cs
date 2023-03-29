using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Salons
{
    public class TimeModeItemConfig : IEntityTypeConfiguration<TimeModeItem>
    {
        public void Configure(EntityTypeBuilder<TimeModeItem> builder)
        {
            builder.Property(p => p.Hour).IsRequired().HasMaxLength(23);
            builder.Property(p => p.Minute).IsRequired().HasMaxLength(59);
            builder.Property(p => p.TimeCreate).HasDefaultValueSql("getdate()");
        }
    }
}