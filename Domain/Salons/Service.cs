using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class Service : BaseProps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }

        //
        public int BarberId { get; set; }
        public Barber Barber { get; set; }
    }
}
