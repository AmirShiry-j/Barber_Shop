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
    public class TurnConfig : IEntityTypeConfiguration<Turn>
    {
        public void Configure(EntityTypeBuilder<Turn> builder)
        {

            var converterSituationEnum = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.EnumToStringConverter<Situation>();
            builder.Property(p => p.Situation).IsRequired().HasConversion(converterSituationEnum);

            builder.Property(p => p.TimeCreate).HasDefaultValueSql("getdate()");
        }
    }
}
