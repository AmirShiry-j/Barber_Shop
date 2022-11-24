using Application.Interfaces.Contexts;
using Domain.Salons;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.Salons;
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
        //Users
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        //Salons
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<SalonImage> SalonImages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            ////Relations
            //Users
            builder.Entity<Token>()
                .HasOne(p => p.User)
                .WithMany(p => p.Tokens)
                .HasForeignKey(p => p.UserId)
                .IsRequired(true);

            builder.Entity<Token>()
                .HasOne(p => p.User)
                .WithMany(p => p.Tokens)
                .HasForeignKey(p => p.UserId)
                .IsRequired(true);

            //Salons
            builder.Entity<User>()
                .HasOne(p => p.Barber)
                .WithOne(p => p.User)
                .HasForeignKey<User>(p=>p.BarberId)
                .IsRequired(false);

            builder.Entity<Salon>()
                .HasMany(p=>p.SalonImages)
                .WithOne()
                .HasForeignKey(p => p.SalonId)
                .IsRequired(true);

            builder.Entity<Salon>()
                .HasOne(p=>p.Owner)
                .WithOne(p=>p.Salon)
                .HasForeignKey<Salon>(p=>p.OwnerId)
                .IsRequired(true);

            builder.Entity<User>()
                .HasOne(p => p.Salon)
                .WithOne(p => p.Owner)
                .HasForeignKey<User>(p => p.SalonId)
                .IsRequired(false);

            //Users
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new TokenConfig());
            //Salons
            builder.ApplyConfiguration(new BarberConfig());
            builder.ApplyConfiguration(new SalonConfig());
            builder.ApplyConfiguration(new SalonImageConfig());


            base.OnModelCreating(builder);
        }
    }
}
