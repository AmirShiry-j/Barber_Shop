using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class FavoriteSalon
    {
        public int Id { get; set; }

        //Nav
        public string UserId { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }
    }
}
