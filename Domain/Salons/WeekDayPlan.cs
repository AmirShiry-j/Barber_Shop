using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Salons
{
    public class WeekDayPlan
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public int BarberId { get; set; }

        public int TimeModeId { get; set; }
        public TimeMode TimeMode { get; set; }
    }
    public enum DayOfWeek
    {
        SAT = 1,
        SUN = 2,
        MON = 3,
        TUES = 4,
        WED = 5,
        THURS = 6,
        FRI = 7,
    }
}
