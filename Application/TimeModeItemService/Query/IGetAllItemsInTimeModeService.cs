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
    public interface IGetAllItemsInTimeModeService
    {
        Task<ResultDto<List<TimeModeItemDto>>> Execute(string UserId, int TimeModeId);

    }
    public class GetAllItemsInTimeModeService : IGetAllItemsInTimeModeService
    {
        private readonly IDataBaseContext _dbContext;
        public GetAllItemsInTimeModeService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto<List<TimeModeItemDto>>> Execute(string UserId, int TimeModeId)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto<List<TimeModeItemDto>>
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            //Check is exist TimeMode
            var timeMode = _dbContext.TimeModes.Where(p => p.Id.Equals(TimeModeId)).Include(p => p.TimeModeItems).FirstOrDefault();
            if (timeMode == null)
            {
                return new ResultDto<List<TimeModeItemDto>>
                {
                    IsSuccess = false,
                    Message = "گروهبندی زمانی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner TimeMode
            if (timeMode.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto<List<TimeModeItemDto>>
                {
                    IsSuccess = false,
                    Message = "شما مالک این گروهبندی زمانی نیستید"
                };
            }

            //map to dto
            var items = timeMode.TimeModeItems.Select(p => new TimeModeItemDto
            {
                Id = p.Id,
                Hour = p.Hour,
                Minute = p.Minute,
                TimeModeId = TimeModeId,
                TimeCreate = p.TimeCreate,
                TimeLastUpdate = p.TimeLastUpdate
            }).ToList();

            return new ResultDto<List<TimeModeItemDto>>
            {
                IsSuccess = true,
                Data = items
            };
        }

    }
    public class TimeModeItemDto
    {
        public int Id { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
        //Nave
        public int TimeModeId { get; set; }
    }
}
