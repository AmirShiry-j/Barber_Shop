using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeService.Command
{
    public interface IAddTimeModeService
    {
        Task<ResultDto<int>> Execute(string UserId, CreateTimeModeDto Dto);
    }
    public class AddTimeModeService : IAddTimeModeService
    {
        private readonly IDataBaseContext _dbContext;
        public AddTimeModeService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<int>> Execute(string UserId, CreateTimeModeDto Dto)
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

            //Check Limited for count TimeMode
            const int Limited = 7;
            var countTimeModes = _dbContext.TimeModes.Where(p => p.BarberId.Equals(barber.Id)).Count();
            if (countTimeModes >= Limited)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = $"شما بیش از {Limited} گروهبندی زمانی نمیتوانید ایجاد کنید"
                };
            }

            //Insert in db
            var newTimeMode = new TimeMode
            {
                BarberId = barber.Id,
                Name = Dto.Name
            };
            _dbContext.TimeModes.Add(newTimeMode);
            _dbContext.SaveChanges();

            return new ResultDto<int>
            {
                IsSuccess = true,
                Data = newTimeMode.Id
            };
        }
    }
    public class CreateTimeModeDto
    {
        public string Name { get; set; }
    }
}
