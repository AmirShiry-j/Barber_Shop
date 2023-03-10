using Domain.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations.Comments
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.BarberId).IsRequired();
            builder.Property(p => p.Text).HasMaxLength(100);
            builder.Property(p => p.TimeCreate).HasDefaultValueSql("getdate()");
        }
    }
}
