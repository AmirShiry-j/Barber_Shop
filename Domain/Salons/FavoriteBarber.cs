using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class FavoriteBarber
    {
        public int Id { get; set; }

        //Nav
        public string UserId { get; set; }
        public int BarberId { get; set; }
        public Barber Barber { get; set; }
    }
}
