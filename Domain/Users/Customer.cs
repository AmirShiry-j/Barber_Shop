using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Customer
    {
        public int Id { get; set; }
        //Navs
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
