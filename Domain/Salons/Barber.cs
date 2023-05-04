using Domain.Common;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class Barber : BaseProps
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }

        //Nav
        public User User { get; set; }
        public string UserId { get; set; }
        public Salon Salon { get; set; }
        public int? SalonId { get; set; }
        public DateTime TimeCreate { get; set; }
        public ICollection<TimeMode> TimeModes { get; set; }
<<<<<<< HEAD
        public ICollection<Service> Services { get; set; }
=======
        public ICollection<WeekDayPlan> WeekDayPlans { get; set; }
>>>>>>> master
    }
}
