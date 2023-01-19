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
    public class SalonConfig : IEntityTypeConfiguration<Salon>
    {
        public void Configure(EntityTypeBuilder<Salon> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PhoneNumber).IsRequired();
        }
    }
}
