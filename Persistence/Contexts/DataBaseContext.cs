using Application.Interfaces.Contexts;
using Domain.Addresses;
using Domain.Salons;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations.Addresses;
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
        public DbSet<Customer> Customers { get; set; }
        //Salons
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<SalonImage> SalonImages { get; set; }
        //Address
        public DbSet<Address> addresses { get; set; }
        public DbSet<United> Uniteds { get; set; }
        public DbSet<City> Cities { get; set; }

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
                .HasForeignKey<User>(p => p.BarberId)
                .IsRequired(false);

            builder.Entity<Salon>()
                .HasMany(p => p.SalonImages)
                .WithOne()
                .HasForeignKey(p => p.SalonId)
                .IsRequired(true);

            //Salon and owner

            builder.Entity<User>()
                .HasOne<Salon>()
                .WithOne(p => p.Owner)
                .HasForeignKey<Salon>(p =>p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Salon>()
            //    .HasOne(p => p.Owner)
            //    .WithOne(p => p.Salon)
            //    .HasForeignKey<Salon>(p => p.OwnerId)
            //    .IsRequired(true)
            //    .OnDelete(DeleteBehavior.SetNull);

            //

            builder.Entity<User>()
                .HasOne(p => p.Customer)
                .WithOne(p => p.User)
                .HasForeignKey<Customer>(p => p.UserId)
                .IsRequired(true);
            //For Address
            builder.Entity<Address>()
                .HasOne(p => p.City)
                .WithMany();

            builder.Entity<Salon>()
                .HasOne(p => p.Address)
                .WithOne()
                .HasForeignKey<Salon>(p => p.AddressId);

            //Users
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new TokenConfig());
            builder.ApplyConfiguration(new CustomerConfig());
            //Salons
            builder.ApplyConfiguration(new BarberConfig());
            builder.ApplyConfiguration(new SalonConfig());
            builder.ApplyConfiguration(new SalonImageConfig());
            //Addresses
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new UnitedConfig());
            builder.ApplyConfiguration(new CityConfig());

            base.OnModelCreating(builder);
        }
    }
}
