using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeItemService.Command
{
    public interface IEditTimeModeItemService
    {
        Task<ResultDto> Execute(string UserId, EditTimeModeItemDto Dto);
    }
    public class EditTimeModeItemService : IEditTimeModeItemService
    {
        private readonly IDataBaseContext _dbContext;
        public EditTimeModeItemService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(string UserId, EditTimeModeItemDto Dto)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            //Check is exist TimeModeItem
            var timeModeItem = _dbContext.TimeModeItems.Where(p => p.Id.Equals(Dto.Id)).Include(p => p.TimeMode).FirstOrDefault();
            if (timeModeItem == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "نوبتی در گروهبندی های زمانی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner TimeModeItem
            if (timeModeItem.TimeMode.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما مالک این نوبت از گروهبندی های زمانی نیستید"
                };
            }

            //Edit and save in db
            timeModeItem.Hour = Dto.Hour;
            timeModeItem.Minute = Dto.Minute;
            timeModeItem.TimeLastUpdate = DateTime.Now;
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class EditTimeModeItemDto
    {
        public int Id { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
