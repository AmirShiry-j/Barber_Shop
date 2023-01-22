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
        public Task<ResultDto> Execute(string UserId, int SalonId);
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
        public async Task<ResultDto> Execute(string UserId, int SalonId)
        {
            //Get salon from db
            var salon = _dbContext.Salons.Find(SalonId);

            //Check is exist
            if (salon == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما سالن آرایشی ثبت نکرده اید"
                };
            }

            //Check user is owner
            if (salon.OwnerId != UserId)
            {

                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این  یوزر مالک آرایشگاه نیست"
                };
            }

            //delete from db
            _dbContext.Salons.Remove(salon);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
            };

        }
    }
}
