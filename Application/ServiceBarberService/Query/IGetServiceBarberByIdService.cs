using Application.Common;
using Application.Interfaces.Contexts;
using Application.TimeModeService.Query;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceBarberService.Query
{
    public interface IGetServiceBarberByIdService
    {
        Task<ResultDto<ServiceDetailDto>> Execute(string UserId, int ServiceId);

    }
    public class GetServiceBarberByIdService : IGetServiceBarberByIdService
    {
        private readonly IDataBaseContext _dbContext;
        public GetServiceBarberByIdService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<ServiceDetailDto>> Execute(string UserId, int ServiceId)
        {
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(UserId)).FirstOrDefault();

            //Check user is barber
            if (barber == null)
            {
                return new ResultDto<ServiceDetailDto>
                {
                    IsSuccess = false,
                    Message = "این یوزر یک آرایشگر نیست"
                };
            }

            //Check is exist Service
            var service = _dbContext.Services.Where(p => p.Id.Equals(ServiceId)).FirstOrDefault();
            if (service == null)
            {
                return new ResultDto<ServiceDetailDto>
                {
                    IsSuccess = false,
                    Message = "خدمتی با آیدی ارسالی موجود نیست"
                };
            }

            //Check barber is Owner Service
            if (service.BarberId.Equals(barber.Id) == false)
            {
                return new ResultDto<ServiceDetailDto>
                {
                    IsSuccess = false,
                    Message = "شما مالک این خدمت نیستید"
                };
            }

            //map to dto
            var serviceDetailDto = new ServiceDetailDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                TimeCreate = service.TimeCreate,
                TimeLastUpdate = service.TimeLastUpdate
            };

            return new ResultDto<ServiceDetailDto>
            {
                IsSuccess = true,
                Data = serviceDetailDto
            };
        }

    }

    public class ServiceDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateTime TimeCreate { get; set; }
        public DateTime? TimeLastUpdate { get; set; }
        public List<Link> Links { get; set; }
    }
}
