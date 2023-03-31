using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayOfWeek = Domain.Salons.DayOfWeek;

namespace Persistence.Configurations.Salons
{
    public class WeekDayPlanConfig : IEntityTypeConfiguration<WeekDayPlan>
    {
        public void Configure(EntityTypeBuilder<WeekDayPlan> builder)
        {
            var converterDayOfWeekEnum = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.EnumToStringConverter<DayOfWeek>();
            builder.Property(p => p.DayOfWeek).IsRequired().HasConversion(converterDayOfWeekEnum);

        }
    }
}
