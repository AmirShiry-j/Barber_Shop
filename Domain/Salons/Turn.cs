using Domain.Common;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class Turn : BaseProps
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BarberId { get; set; }
        public int? TimeModeItemId { get; set; }
        public DateTime DateTime { get; set; }
        public Situation Situation { get; set; }
        public DateTime TimeCreate { get; set; }

        //Nav 
        public Customer Customer { get; set; }
        public Barber Barber { get; set; }
        public TimeModeItem TimeModeItem { get; set; }
    }

    public enum Situation
    {
        Expectant = 1,
        done = 2
    }
}
