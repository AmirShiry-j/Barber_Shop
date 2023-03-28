using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeService.Query
{
    public interface IGetTimeModeByIdService
    {
        Task<ResultDto<TimeModeDetailDto>> Execute(string UserId, int TimeModeId);

    }
    public class GetTimeModeByIdService : IGetTimeModeByIdService
    {
        private readonly IDataBaseContext _dbContext;
        public GetTimeModeByIdService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<TimeModeDetailDto>> Execute(string UserId, int TimeModeId)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto<TimeModeDetailDto>
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            //Check is exist TimeMode
            var timeMode = _dbContext.TimeModes.Where(p => p.Id.Equals(TimeModeId)).FirstOrDefault();
            if (timeMode == null)
            {
                return new ResultDto<TimeModeDetailDto>
                {
                    IsSuccess = false,
                    Message = "گروهبندی زمانی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner TimeMode
            if (timeMode.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto<TimeModeDetailDto>
                {
                    IsSuccess = false,
                    Message = "شما مالک این گروهبندی زمانی نیستید"
                };
            }

            //map to dto
            var timeModeDetailDto = new TimeModeDetailDto
            {
                Id = timeMode.Id,
                Name = timeMode.Name,
                TimeCreate = timeMode.TimeCreate,
                TimeLastUpdate = timeMode.TimeLastUpdate
            };

            return new ResultDto<TimeModeDetailDto>
            {
                IsSuccess = true,
                Data = timeModeDetailDto
            };
        }
    }
    public class TimeModeDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
    }
}
