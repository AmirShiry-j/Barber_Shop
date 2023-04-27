using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceBarberService.Command
{
    public interface IEditServiceBarberService
    {
        Task<ResultDto> Execute(string UserId, EditServiceDto Dto);

    }
    public class EditServiceBarberService : IEditServiceBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public EditServiceBarberService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(string UserId, EditServiceDto Dto)
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
            var service = _dbContext.Services.Where(p => p.Id.Equals(Dto.Id)).FirstOrDefault();
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

            //Update in db
            service.Name = Dto.Name;
            service.Description = Dto.Description;
            service.Price = Dto.Price;
            service.TimeLastUpdate = DateTime.Now;
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
    public class EditServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
