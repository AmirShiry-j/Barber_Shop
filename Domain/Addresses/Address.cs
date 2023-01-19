using Domain.Salons;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Addresses
{
    public class Address
    {
        public int Id { get; set; }

        public string FullAddress { get; set; }

        //
        public City City { get; set; }
        public int CityId { get; set; }
    }
}
