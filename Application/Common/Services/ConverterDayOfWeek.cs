using Domain.Salons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Application.Common.Services
{
    public class ConverterDayOfWeek
    {
        public static Domain.Salons.DayOfWeek ConvertOfficialToCustomize(System.DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case System.DayOfWeek.Saturday:
                    return Domain.Salons.DayOfWeek.SAT;
                    break;
                case System.DayOfWeek.Sunday:
                    return Domain.Salons.DayOfWeek.SUN;
                    break;
                case System.DayOfWeek.Monday:
                    return Domain.Salons.DayOfWeek.MON;
                    break;
                case System.DayOfWeek.Tuesday:
                    return Domain.Salons.DayOfWeek.TUES;
                    break;
                case System.DayOfWeek.Wednesday:
                    return Domain.Salons.DayOfWeek.WED;
                    break;
                case System.DayOfWeek.Thursday:
                    return Domain.Salons.DayOfWeek.THURS;
                    break;
                case System.DayOfWeek.Friday:
                    return Domain.Salons.DayOfWeek.FRI;
                    break;

                default:
                    return Domain.Salons.DayOfWeek.FRI;
                    break;
            }
        }
        public static System.DayOfWeek ConvertCustomizeToOfficial(Domain.Salons.DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case Domain.Salons.DayOfWeek.SAT:
                    return System.DayOfWeek.Saturday;
                    break;
                case Domain.Salons.DayOfWeek.SUN:
                    return System.DayOfWeek.Sunday;
                    break;
                case Domain.Salons.DayOfWeek.MON:
                    return System.DayOfWeek.Monday;
                    break;
                case Domain.Salons.DayOfWeek.TUES:
                    return System.DayOfWeek.Tuesday;
                    break;
                case Domain.Salons.DayOfWeek.WED:
                    return System.DayOfWeek.Wednesday;
                    break;
                case Domain.Salons.DayOfWeek.THURS:
                    return System.DayOfWeek.Thursday;
                    break;
                case Domain.Salons.DayOfWeek.FRI:
                    return System.DayOfWeek.Friday;
                    break;

                default:
                    return System.DayOfWeek.Friday;
                    break;
            }
        }


    }
}