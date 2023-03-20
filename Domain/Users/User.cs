using Domain.Common;
using Domain.Salons;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User : IdentityUser, BaseProps
    {
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public string ImageName { get; set; }
        public bool AcceptedAsBarber { get; set; }
        //Navs
        public ICollection<Token> Tokens { get; set; }
        //
        public Barber Barber { get; set; }
        public int? BarberId { get; set; }
        //
        public Customer Customer { get; set; }
        public int? CustomerId { get; set; }
        public DateTime TimeCreate { get; set; }

        public ICollection<FavoriteBarber> FavoriteBarbers { get; set; }
        public ICollection<FavoriteSalon> FavoriteSalons { get; set; }
    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
