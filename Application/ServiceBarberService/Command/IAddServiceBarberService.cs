using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceBarberService.Command
{
    public interface IAddServiceBarberService
    {
        Task<ResultDto<int>> Execute(string UserId, CreateServiceDto Dto);
    }

    public class AddServiceBarberService : IAddServiceBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public AddServiceBarberService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<int>> Execute(string UserId, CreateServiceDto Dto)
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

            //Check Limited for count Service
            const int Limited = 40;
            var countTimeModes = _dbContext.Services.Where(p => p.BarberId.Equals(barber.Id)).Count();
            if (countTimeModes >= Limited)
            {
                return new ResultDto<int>
                {
                    IsSuccess = false,
                    Message = $"شما بیش از {Limited} مورد خدمات نمیتوانید ایجاد کنید"
                };
            }

            //Map to Entity
            var newService = new Service
            {
                BarberId = barber.Id,
                Name = Dto.Name,
                Description = Dto.Description,
                Price = Dto.Price
            };

            //Insert in db
            _dbContext.Services.Add(newService);
            _dbContext.SaveChanges();

            return new ResultDto<int>
            {
                Data = newService.Id,
                IsSuccess = true
            };
        }
    }

    public class CreateServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
