using Application.Common;
using Application.Common.Services;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TurnService.Query
{
    public interface IGetTurnOneDay
    {
        Task<ResultDto<List<TurnDto>>> Execute(int BarberId, DateTime DateTime);
    }

    public class GetTurnOneDay : IGetTurnOneDay
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IConfiguration _configuration;
        public GetTurnOneDay(IDataBaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<ResultDto<List<TurnDto>>> Execute(int BarberId, DateTime DateTime)
        {
            var maxNextDayForTurn = int.Parse(_configuration["MaxNextDayForTurn"].ToString());

            //Check Datetime
            if (DateTime.Now.AddDays(maxNextDayForTurn) < DateTime)
            {
                return new ResultDto<List<TurnDto>>
                {
                    Message = "شما از حداکثر بازه زمانی که میتوان نوبت گرفت جلوتر رفتید"
                };
            }
            else if (DateTime.Now > DateTime)
            {
                return new ResultDto<List<TurnDto>>
                {
                    Message = "زمانی که برای گرفتن نوبت ارسال شده متعلق به گذشته است"
                };
            }

            //Check is exist barber
            var barber = _dbContext.Barbers.Find(BarberId);
            if (barber == null)
            {
                return new ResultDto<List<TurnDto>>
                {
                    Message = "آرایشگری با آیدی ارسالی موجود نیست"
                }; ;
            }

            //Get dayPlan for Turns
            var dayOfWeek = ConverterDayOfWeek.ConvertOfficialToCustomize(DateTime.DayOfWeek);
            var dayPlan = _dbContext.WeekDayPlans
                .Where(p => p.BarberId.Equals(BarberId) && p.DayOfWeek.Equals(dayOfWeek))
                .Include(p => p.TimeMode)
                .ThenInclude(p => p.TimeModeItems)
                .FirstOrDefault();

            //Chekc is exist plan
            if (dayPlan == null)
            {
                return new ResultDto<List<TurnDto>>
                {
                    Message = "آرایشگر به این روز برنامه ای اختصاص نداده است"
                };
            }
            else if (dayPlan.TimeMode.TimeModeItems.Any() == false)
            {
                return new ResultDto<List<TurnDto>>
                {
                    Message = "به این روز نوبتی اختصاص داده نشده است"
                };
            }

            //set turns
            var turns = dayPlan.TimeMode.TimeModeItems.Select(p => new TurnDto
            {
                Id = p.Id,
                Time = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day, p.Hour, p.Minute, 0)
            })
                .OrderBy(p => p.Time)
            .ToList();

            return new ResultDto<List<TurnDto>>
            {
                IsSuccess = true,
                Data = turns
            };
        }
    }
    public class TurnDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
    }
}
