using Application.Common;
using Application.Interfaces.Contexts;
using Application.TimeModeService.Query;
using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeItemService.Query
{
    public interface IGetTimeModeItemService
    {
        Task<ResultDto<TimeModeItemDetailDto>> Execute(string UserId, int TimeModeItemId);

    }
    public class GetTimeModeItemService : IGetTimeModeItemService
    {
        private readonly IDataBaseContext _dbContext;
        public GetTimeModeItemService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<TimeModeItemDetailDto>> Execute(string UserId, int TimeModeItemId)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto<TimeModeItemDetailDto>
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            //Check is exist TimeModeItem
            var timeModeItem = _dbContext.TimeModeItems.Where(p => p.Id.Equals(TimeModeItemId)).Include(p => p.TimeMode).FirstOrDefault();
            if (timeModeItem == null)
            {
                return new ResultDto<TimeModeItemDetailDto>
                {
                    IsSuccess = false,
                    Message = "نوبتی در گروهبندی های زمانی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner TimeModeItem
            if (timeModeItem.TimeMode.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto<TimeModeItemDetailDto>
                {
                    IsSuccess = false,
                    Message = "شما مالک این نوبت از گروهبندی های زمانی نیستید"
                };
            }

            //Map to dto
            var timeModeItemDetailDto = new TimeModeItemDetailDto()
            {
                Id = timeModeItem.Id,
                Hour = timeModeItem.Hour,
                Minute = timeModeItem.Minute,
                TimeCreate = timeModeItem.TimeCreate,
                TimeLastUpdate = timeModeItem.TimeLastUpdate,
                TimeModeId = timeModeItem.TimeModeId,
                TimeModeName = timeModeItem.TimeMode.Name
            };

            return new ResultDto<TimeModeItemDetailDto>
            {
                IsSuccess = true,
                Data = timeModeItemDetailDto
            };
        }
    }
    public class TimeModeItemDetailDto
    {
        public int Id { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
        public int TimeModeId { get; set; }
        public string TimeModeName { get; set; }
        public List<Link> Links { get; set; }
    }
}
