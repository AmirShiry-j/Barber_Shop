using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Addresses
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //
        public United United { get; set; }
        public int UnitedId { get; set; }

    }
}
