using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TurnService.Command
{
    public interface ITakeTurnService
    {
        Task<ResultDto<int>> Execute(TurnCreateDto TurnCreateDto);
    }

    public class TakeTurnService : ITakeTurnService
    {
        private readonly IDataBaseContext _dbContext;
        public TakeTurnService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto<int>> Execute(TurnCreateDto TurnCreateDto)
        {
            //Find customer
            var customer = _dbContext.Customers.Where(p => p.UserId.Equals(TurnCreateDto.UserId)).FirstOrDefault();

            //Check has customer
            if (customer == null)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "کاربر دارای کد مشتری نیست"
                };
            }

            //Find Barber
            var barber = _dbContext.Barbers.Where(p => p.Id.Equals(TurnCreateDto.BarberId)).Include(p => p.User).FirstOrDefault();

            //Check has berber
            if (barber == null)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "آرایشگری با آیدی ارسالی وجود ندارد"
                };
            }

            //Find TimeModeItem
            var timeModeItem = _dbContext.TimeModeItems.Where(p => p.Id.Equals(TurnCreateDto.TimeModeItemId)).Include(p => p.TimeMode)
                .ThenInclude(p => p.WeekDayPlans)
                .FirstOrDefault();

            //Check has TimeModeItem
            if (timeModeItem == null)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "نوبتی با آیدی ارسالی وجود ندارد"
                };
            }

            //Check Exist in Db 
            var turnBefore = _dbContext.Turns.Where(p => p.BarberId == TurnCreateDto.BarberId && p.DateTime == TurnCreateDto.DateTime && p.TimeModeItemId == TurnCreateDto.TimeModeItemId).FirstOrDefault();
            if (turnBefore != null)
            {
                if (turnBefore.CustomerId == customer.Id)
                {
                    return new ResultDto<int>
                    {
                        IsSuccess = false,
                        Message = "از قبل نوبتی با این مشخصات توسط شما گرفته شده است"
                    };
                }
                else
                {
                    return new ResultDto<int>
                    {
                        IsSuccess = false,
                        Message = "از قبل نوبتی با این مشخصات توسط شخص دیگری گرفته شده است"
                    };
                }
            }


            ////Check Conformity TimeModeItem To DateTime Turn
            //For hour and minute
            if (!(timeModeItem.Hour == TurnCreateDto.DateTime.Hour && timeModeItem.Minute == TurnCreateDto.DateTime.Minute))
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "زمان ارسالی به نوبت زمانی انتخاب شده نمیخورد"
                };
            }
            //For dayOfWeek
            var dayOfWeekTurn = ConverterDayOfWeek.ConvertOfficialToCustomize(TurnCreateDto.DateTime.DayOfWeek);
            var HasTurnInDayOfWeek = timeModeItem.TimeMode.WeekDayPlans.Any(p => p.DayOfWeek == dayOfWeekTurn);
            if (!HasTurnInDayOfWeek)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "زمان ارسالی به نوبت زمانی انتخاب شده نمیخورد"
                };
            }
            

            //Insert in db
            var newTurn = new Turn
            {
                BarberId = TurnCreateDto.BarberId,
                CustomerId = customer.Id,
                DateTime = TurnCreateDto.DateTime,
                TimeModeItemId = TurnCreateDto.TimeModeItemId,
                Situation = Situation.Expectant
            };
            _dbContext.Turns.Add(newTurn);
            _dbContext.SaveChanges();

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = newTurn.Id
            };
        }
    }
    public class TurnCreateDto
    {
        public string UserId { get; set; }
        public int BarberId { get; set; }
        public int? TimeModeItemId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
