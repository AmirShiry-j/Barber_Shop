using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonsService.Command
{
    public interface IDeleteSalonService
    {
        public Task<ResultDto<List<string>>> Execute(string UserId, int SalonId);
    }

    public class DeleteSalonService : IDeleteSalonService
    {
        ILogger<AddSalonService> _logger;
        private readonly IDataBaseContext _dbContext;
        public DeleteSalonService(IDataBaseContext dbContext, ILogger<AddSalonService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<ResultDto<List<string>>> Execute(string UserId, int SalonId)
        {
            //Get salon from db
            var salon = _dbContext.Salons.Where(p => p.Id.Equals(SalonId)).Include(p => p.SalonImages).FirstOrDefault();

            //Check is exist
            if (salon == null)
            {
                return new ResultDto<List<string>>
                {
                    IsSuccess = false,
                    Message = "شما سالن آرایشی ثبت نکرده اید"
                };
            }

            //Check user is owner
            if (salon.OwnerId != UserId)
            {

                return new ResultDto<List<string>>
                {
                    IsSuccess = false,
                    Message = "این  یوزر مالک آرایشگاه نیست"
                };
            }

            //delete from db
            _dbContext.Salons.Remove(salon);
            _dbContext.SaveChanges();

            return new ResultDto<List<string>>
            {
                IsSuccess = true,
                Data = salon.SalonImages?.Select(p => p.Name).ToList()
            };

        }
    }
}
