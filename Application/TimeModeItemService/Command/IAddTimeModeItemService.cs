using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeItemService.Command
{
    public interface IAddTimeModeItemService
    {
        Task<ResultDto<int>> Execute(string UserId, CreateTimeModeItemDto Dto);

    }
    public class AddTimeModeItemService : IAddTimeModeItemService
    {
        private readonly IDataBaseContext _dbContext;
        public AddTimeModeItemService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<int>> Execute(string UserId, CreateTimeModeItemDto Dto)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            //Check TimeMode is exist
            var timeMode = _dbContext.TimeModes.Where(p => p.Id.Equals(Dto.TimeModeId)).FirstOrDefault();
            if (timeMode == null)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "گروهبندی زمانی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner TimeMode
            if (timeMode.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = "شما مالک این گروهبندی زمانی نیستید"
                };
            }

            //Check Limited for count TimeModeItems
            const int Limited = 24;
            var countItems = _dbContext.TimeModeItems.Where(p => p.TimeModeId.Equals(Dto.TimeModeId)).Count();
            if (countItems >= Limited)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = $"گروهبندی زمانی نمیتواند بیش از {Limited} داشته باشد"
                };
            }

            //Insert in db
            var newItem = new TimeModeItem
            {
                Hour = Dto.Hour,
                Minute = Dto.Minute,
                TimeModeId = Dto.TimeModeId,
            };
            _dbContext.TimeModeItems.Add(newItem);
            _dbContext.SaveChanges();

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = newItem.Id
            };
        }
    }
    public class CreateTimeModeItemDto
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int TimeModeId { get; set; }
    }
}
