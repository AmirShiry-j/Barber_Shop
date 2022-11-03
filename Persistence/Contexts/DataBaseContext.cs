using Application.Interfaces.Contexts;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class DataBaseContext : IdentityDbContext<User, Role, string>, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ////Relations
            builder.Entity<Token>()
                .HasOne(p => p.User)
                .WithMany(p => p.Tokens)
                .HasForeignKey(p => p.UserId)
                .IsRequired(true);




            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new TokenConfig());


            base.OnModelCreating(builder);
        }
    }
}
