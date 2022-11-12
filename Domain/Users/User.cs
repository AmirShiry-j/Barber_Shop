using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public Hewitt Hewitt { get; set; }
        public bool AcceptedAsBarber { get; set; }
        //Navs
        public ICollection<Token> Tokens { get; set; }

    }
    public enum Gender
    {
        Male=1,
        Female=2
    }
    public enum Hewitt
    {
        Customer=1,
        Barber=2
    }
}
