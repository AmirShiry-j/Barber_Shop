using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeService.Command
{
    public interface IDeleteTimeModeService
    {
        Task<ResultDto> Execute(string UserId, int TimeModeId);

    }
    public class DeleteTimeModeService : IDeleteTimeModeService
    {
        private readonly IDataBaseContext _dbContext;
        public DeleteTimeModeService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(string UserId, int TimeModeId)
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

            //Check is exist TimeMode
            var timeMode = _dbContext.TimeModes.Where(p => p.Id.Equals(TimeModeId)).FirstOrDefault();
            if (timeMode == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "گروهبندی زمانی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner TimeMode
            if (timeMode.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما مالک این گروهبندی زمانی نیستید"
                };
            }

            //Delete in db
            _dbContext.TimeModes.Remove(timeMode);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
