using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Users
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var converterGenderEnum = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.EnumToStringConverter<Gender>();

            builder.Property(p => p.FullName).IsRequired().HasMaxLength(50);
            builder.Property(p => p.ImageName).IsRequired(false);
            builder.Property(p => p.Gender).IsRequired().HasConversion(converterGenderEnum);
            builder.Property(p => p.TimeCreate).HasDefaultValueSql("getdate()");
        }
    }
}
