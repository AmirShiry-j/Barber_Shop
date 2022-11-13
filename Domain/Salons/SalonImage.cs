using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class SalonImage
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Nav rel
        public int SalonId { get; set; }

    }
}
