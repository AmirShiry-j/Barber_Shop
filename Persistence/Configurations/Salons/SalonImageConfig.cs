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
    public class SalonImageConfig : IEntityTypeConfiguration<SalonImage>
    {
        public void Configure(EntityTypeBuilder<SalonImage> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.SalonId).IsRequired();
        }
    }
}
