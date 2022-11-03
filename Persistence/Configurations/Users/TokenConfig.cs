using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Users
{
    public class TokenConfig : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.Property(p => p.TokenHash).IsRequired();
            builder.Property(p => p.TokenExpireTime).IsRequired();
            builder.Property(p => p.RefreshTokenHash).IsRequired();
            builder.Property(p => p.RefreshTokenExpireTime).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.CreateTime).HasDefaultValueSql("getdate()");
        }
    }
}
