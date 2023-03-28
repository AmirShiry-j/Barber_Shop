using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TimeModeService.Command
{

    public interface IEditTimeModeService
    {
        Task<ResultDto> Execute(string UserId, EditTimeModeDto Dto);

    }
    public class EditTimeModeService : IEditTimeModeService
    {
        private readonly IDataBaseContext _dbContext;
        public EditTimeModeService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto> Execute(string UserId, EditTimeModeDto Dto)
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
            var timeMode = _dbContext.TimeModes.Where(p => p.Id.Equals(Dto.Id)).FirstOrDefault();
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

            //Update in Db
            timeMode.TimeLastUpdate = DateTime.Now;
            timeMode.Name = Dto.Name;
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class EditTimeModeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
