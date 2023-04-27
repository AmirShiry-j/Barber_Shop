using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }

        //
        public int BarberId { get; set; }
        public Barber Barber { get; set; }
    }
}
