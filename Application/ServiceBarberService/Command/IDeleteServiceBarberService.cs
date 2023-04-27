using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceBarberService.Command
{
    public interface IDeleteServiceBarberService
    {
        Task<ResultDto> Execute(string UserId, int ServiceId);

    }
    public class DeleteServiceBarberService : IDeleteServiceBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public DeleteServiceBarberService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(string UserId, int ServiceId)
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

            //Check is exist Service
            var service = _dbContext.Services.Where(p => p.Id.Equals(ServiceId)).FirstOrDefault();
            if (service == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "خدمتی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner Service
            if (service.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما مالک این خدمت نیستید"
                };
            }

            //Delete in db
            _dbContext.Services.Remove(service);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
