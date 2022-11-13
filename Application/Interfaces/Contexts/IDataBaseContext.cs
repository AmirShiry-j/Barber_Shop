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
        //Salons
        DbSet<Salon> Salons { get; set; }
        DbSet<Barber> Barbers { get; set; }
        DbSet<SalonImage> SalonImages { get; set; }
        int SaveChanges();
    }
}
