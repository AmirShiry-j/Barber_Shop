using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceBarberService.Query
{
    public interface IGetAllServiceBarberForBarber_Service
    {
        Task<ResultDto<List<ServiceDto>>> Execute(string UserId);
    }
    public class GetAllServiceBarberForBarber_Service : IGetAllServiceBarberForBarber_Service
    {
        private readonly IDataBaseContext _dbContext;
        public GetAllServiceBarberForBarber_Service(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<List<ServiceDto>>> Execute(string UserId)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto<List<ServiceDto>>
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }


            //Get data from db
            var services = _dbContext.Services.Where(p => p.BarberId.Equals(barber.Id)).Select(p => new ServiceDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                TimeCreate = p.TimeCreate,
                TimeLastUpdate = p.TimeLastUpdate
            })
            .OrderBy(p => p.TimeCreate)
            .ToList();

            return new ResultDto<List<ServiceDto>>
            {
                IsSuccess = true,
                Data = services
            };
        }
    }

    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
        public Link Link { get; set; }
    }
}
