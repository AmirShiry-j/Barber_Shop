using Domain.Addresses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Addresses
{
    class UnitedConfig : IEntityTypeConfiguration<United>
    {
        public void Configure(EntityTypeBuilder<United> builder)
        {
            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        }
    }
}
