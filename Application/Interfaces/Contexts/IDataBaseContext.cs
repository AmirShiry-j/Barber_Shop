using Domain.Addresses;
using Domain.Comments;
using Domain.Salons;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        //Users
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Token> Tokens { get; set; }
        DbSet<Customer> Customers { get; set; }
        //Salons
        DbSet<Salon> Salons { get; set; }
        DbSet<Barber> Barbers { get; set; }
        DbSet<SalonImage> SalonImages { get; set; }
        DbSet<FavoriteBarber> FavoriteBarbers { get; set; }
        DbSet<FavoriteSalon> FavoriteSalons { get; set; }
        DbSet<TimeMode> TimeModes { get; set; }
        DbSet<TimeModeItem> TimeModeItems { get; set; }
<<<<<<< HEAD
        DbSet<Service> Services { get; set; }
=======
        DbSet<WeekDayPlan> WeekDayPlans { get; set; }
>>>>>>> master
        //Address
        public DbSet<Address> addresses { get; set; }
        public DbSet<United> Uniteds { get; set; }
        public DbSet<City> Cities { get; set; }
        //Comments
        public DbSet<Comment> Comments { get; set; }

        int SaveChanges();
    }
}
