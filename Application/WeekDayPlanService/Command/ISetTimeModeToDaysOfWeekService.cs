using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WeekDayPlanService.Command
{
    public interface ISetTimeModeToDaysOfWeekService
    {
        Task<ResultDto> Execute(SetPlanDto SetPlanDto);
    }
    public class SetTimeModeToDaysOfWeekService : ISetTimeModeToDaysOfWeekService
    {
        private readonly IDataBaseContext _dbContext;
        public SetTimeModeToDaysOfWeekService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(SetPlanDto SetPlanDto)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(SetPlanDto.UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            var hasTimeMode = _dbContext.TimeModes.Where(p => p.BarberId.Equals(barber.Id) && p.Id.Equals(SetPlanDto.TimeModeId)).Any();
            if (!hasTimeMode)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "دسته بندی زمانی متعلق به شما با آیدی ارسالی موجود نیست"
                };
            }

            //Get days for set plan from dto
            var days = new List<string>();
            if (SetPlanDto.DaysOfWeek == "*")
            {
                days = new List<string>
                    {
                        "SAT","SUN","MON","TUES","WED","THURS","FRI"
                    };

                if (string.IsNullOrWhiteSpace(SetPlanDto.Excepts) == false)
                {
                    var Excepts = SetPlanDto.Excepts?.Split(",").ToList();

                    days = days.Where(p => Excepts.Contains(p) == false).ToList();
                }
            }
            else
            {
                days = SetPlanDto.DaysOfWeek.Split(",").ToList();
            }

            //Define Before WeekDayPlan s for delete in db
            var beforePlans = new List<WeekDayPlan>();

            //Define New WeekDayPlan s
            var newPlans = new List<WeekDayPlan>();
            foreach (var day in days)
            {
                switch (day)
                {
                    case "SAT":
                        {
                            newPlans.Add(new WeekDayPlan { BarberId = barber.Id, TimeModeId = SetPlanDto.TimeModeId, DayOfWeek = Domain.Salons.DayOfWeek.SAT });

                            var planBefore = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.SAT).FirstOrDefault();
                            if (planBefore != null)
                            {
                                beforePlans.Add(planBefore);
                            }
                        }
                        break;

                    case "SUN":
                        {
                            newPlans.Add(new WeekDayPlan { BarberId = barber.Id, TimeModeId = SetPlanDto.TimeModeId, DayOfWeek = Domain.Salons.DayOfWeek.SUN });

                            var planBefore = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.SUN).FirstOrDefault();
                            if (planBefore != null)
                            {
                                beforePlans.Add(planBefore);
                            }
                        }
                        break;

                    case "MON":
                        {
                            newPlans.Add(new WeekDayPlan { BarberId = barber.Id, TimeModeId = SetPlanDto.TimeModeId, DayOfWeek = Domain.Salons.DayOfWeek.MON });

                            var planBefore = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.MON).FirstOrDefault();
                            if (planBefore != null)
                            {
                                beforePlans.Add(planBefore);
                            }
                        }
                        break;

                    case "TUES":
                        {
                            newPlans.Add(new WeekDayPlan { BarberId = barber.Id, TimeModeId = SetPlanDto.TimeModeId, DayOfWeek = Domain.Salons.DayOfWeek.TUES });

                            var planBefore = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.TUES).FirstOrDefault();
                            if (planBefore != null)
                            {
                                beforePlans.Add(planBefore);
                            }
                        }
                        break;


                    case "WED":
                        {
                            newPlans.Add(new WeekDayPlan { BarberId = barber.Id, TimeModeId = SetPlanDto.TimeModeId, DayOfWeek = Domain.Salons.DayOfWeek.WED });

                            var planBefore = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.WED).FirstOrDefault();
                            if (planBefore != null)
                            {
                                beforePlans.Add(planBefore);
                            }
                        }
                        break;

                    case "THURS":
                        {
                            newPlans.Add(new WeekDayPlan { BarberId = barber.Id, TimeModeId = SetPlanDto.TimeModeId, DayOfWeek = Domain.Salons.DayOfWeek.THURS });

                            var planBefore = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.THURS).FirstOrDefault();
                            if (planBefore != null)
                            {
                                beforePlans.Add(planBefore);
                            }
                        }
                        break;

                    case "FRI":
                        {
                            newPlans.Add(new WeekDayPlan { BarberId = barber.Id, TimeModeId = SetPlanDto.TimeModeId, DayOfWeek = Domain.Salons.DayOfWeek.FRI });

                            var planBefore = _dbContext.WeekDayPlans.Where(p => p.BarberId.Equals(barber.Id) && p.DayOfWeek == Domain.Salons.DayOfWeek.FRI).FirstOrDefault();
                            if (planBefore != null)
                            {
                                beforePlans.Add(planBefore);
                            }
                        }
                        break;
                }
            }

            //Insert in db
            _dbContext.WeekDayPlans.RemoveRange(beforePlans);
            _dbContext.WeekDayPlans.AddRange(newPlans);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class SetPlanDto
    {
        public string UserId { get; set; }
        public int TimeModeId { get; set; }
        public string DaysOfWeek { get; set; }
        public string? Excepts { get; set; }
    }
}
