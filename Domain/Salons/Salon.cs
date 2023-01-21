using Domain.Addresses;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Telphone { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        
        //Nav
        public ICollection<Barber> Barbers { get; set; }
        public ICollection<SalonImage> SalonImages { get; set; }
        public User Owner { get; set; }
        public string OwnerId { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
    }
}
