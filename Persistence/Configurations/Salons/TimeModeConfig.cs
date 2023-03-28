using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Salons
{
    public class TimeModeConfig : IEntityTypeConfiguration<TimeMode>
    {
        public void Configure(EntityTypeBuilder<TimeMode> builder)
        {
            builder.Property(p => p.BarberId).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.TimeCreate).HasDefaultValueSql("getdate()");
        }
    }
}
