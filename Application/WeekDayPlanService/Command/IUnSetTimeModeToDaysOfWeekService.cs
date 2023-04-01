using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WeekDayPlanService.Command
{
    public interface IUnSetTimeModeToDaysOfWeekService
    {
        Task<ResultDto> Execute(UnSetPlanDto UnSetPlanDto);
    }
    public class UnSetTimeModeToDaysOfWeekService : IUnSetTimeModeToDaysOfWeekService
    {
        private readonly IDataBaseContext _dbContext;
        public UnSetTimeModeToDaysOfWeekService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(UnSetPlanDto UnSetPlanDto)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UnSetPlanDto.UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            var hasTimeMode = _dbContext.TimeModes.Where(p => p.BarberId.Equals(barber.Id) && p.Id.Equals(UnSetPlanDto.TimeModeId)).Any();
            if (!hasTimeMode)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دسته بندی زمانی متعلق به شما با آیدی ارسالی موجود نیست"
                };
            }

            //Get days for unset plan from dto
            var days = new List<string>();
            if (UnSetPlanDto.DaysOfWeek == "*")
            {
                days = new List<string>
                    {
                        "SAT","SUN","MON","TUES","WED","THURS","FRI"
                    };

                if (string.IsNullOrWhiteSpace(UnSetPlanDto.Excepts) == false)
                {
                    var Excepts = UnSetPlanDto.Excepts?.Split(",").ToList();

                    days = days.Where(p => Excepts.Contains(p) == false).ToList();
                }
            }
            else
            {
                days = UnSetPlanDto.DaysOfWeek.Split(",").ToList();
            }

            //Define WeekDayPlan s for delete in db
            var plans = new List<WeekDayPlan>();

            foreach (var day in days)
            {
                switch (day)
                {
                    case "SAT":
                        {
                            var plan = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.SAT).FirstOrDefault();
                            if (plan != null)
                            {
                                plans.Add(plan);
                            }
                        }
                        break;

                    case "SUN":
                        {
                            var plan = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.SUN).FirstOrDefault();
                            if (plan != null)
                            {
                                plans.Add(plan);
                            }
                        }
                        break;

                    case "MON":
                        {
                            var plan = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.MON).FirstOrDefault();
                            if (plan != null)
                            {
                                plans.Add(plan);
                            }
                        }
                        break;

                    case "TUES":
                        {
                            var plan = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.TUES).FirstOrDefault();
                            if (plan != null)
                            {
                                plans.Add(plan);
                            }
                        }
                        break;


                    case "WED":
                        {
                            var plan = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.WED).FirstOrDefault();
                            if (plan != null)
                            {
                                plans.Add(plan);
                            }
                        }
                        break;

                    case "THURS":
                        {
                            var plan = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.THURS).FirstOrDefault();
                            if (plan != null)
                            {
                                plans.Add(plan);
                            }
                        }
                        break;

                    case "FRI":
                        {
                            var plan = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.FRI).FirstOrDefault();
                            if (plan != null)
                            {
                                plans.Add(plan);
                            }
                        }
                        break;
                }
            }

            //Delete in db
            _dbContext.WeekDayPlans.RemoveRange(plans);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class UnSetPlanDto
    {
        public string UserId { get; set; }
        public int TimeModeId { get; set; }
        public string DaysOfWeek { get; set; }
        public string? Excepts { get; set; }
    }
}
