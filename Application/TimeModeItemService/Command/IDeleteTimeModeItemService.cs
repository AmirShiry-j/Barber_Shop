using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeItemService.Command
{
    public interface IDeleteTimeModeItemService
    {
        Task<ResultDto> Execute(string UserId, int TimeModeItemId);
    }

    public class DeleteTimeModeItemService : IDeleteTimeModeItemService
    {
        private readonly IDataBaseContext _dbContext;
        public DeleteTimeModeItemService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto> Execute(string UserId, int TimeModeItemId)
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
            var timeModeItem = _dbContext.TimeModeItems.Where(p => p.Id.Equals(TimeModeItemId)).Include(p => p.TimeMode).FirstOrDefault();
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


            //Remove in db
            _dbContext.TimeModeItems.Remove(timeModeItem);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
