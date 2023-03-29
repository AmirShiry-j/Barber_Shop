using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeService.Query
{
    public interface IGetAllTimeModeForBarberService
    {
        Task<ResultDto<List<TimeModeDto>>> Execute(string UserId);

    }
    public class GetAllTimeModeForBarberService : IGetAllTimeModeForBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public GetAllTimeModeForBarberService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<List<TimeModeDto>>> Execute(string UserId)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto<List<TimeModeDto>>
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }


            //Get data from db
            var timeModes = _dbContext.TimeModes.Where(p => p.BarberId.Equals(barber.Id)).Select(p => new TimeModeDto
            {
                Id = p.Id,
                Name = p.Name,
                TimeCreate = p.TimeCreate,
                TimeLastUpdate = p.TimeLastUpdate
            })
            .OrderBy(p => p.TimeCreate)
            .ToList();

            return new ResultDto<List<TimeModeDto>>
            {
                IsSuccess = true,
                Data = timeModes
            };
        }
    }
    public class TimeModeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
        public Link Link { get; set; }
    }
}
