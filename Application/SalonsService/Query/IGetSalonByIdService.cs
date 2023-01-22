using Application.Common;
using Application.Interfaces.Contexts;
using Application.SalonsService.Command;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonsService.Query
{
    public interface IGetSalonByIdService
    {
        public Task<ResultDto<SalonDto>> Execute(int Id);
    }
    public class GetSalonByIdService : IGetSalonByIdService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public GetSalonByIdService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultDto<SalonDto>> Execute(int Id)
        {
            //Get Salon
            var salon = _dbContext.Salons.Where(p => p.Id.Equals(Id))
                .Include(p => p.Owner)
                .Include(p => p.Address)
                .ThenInclude(p => p.City)
                .ThenInclude(p => p.United)
                .FirstOrDefault();

            //Check is exist
            if (salon != null)
            {
                //Map to dto
                var dto = _mapper.Map<SalonDto>(salon);

                return new ResultDto<SalonDto>
                {
                    IsSuccess = true,
                    Data = dto
                };
            }
            else
            {
                return new ResultDto<SalonDto>
                {
                    IsSuccess = false,
                    Message = "سالن آرایشی با این آیدی یافت نشد"
                };
            }
        }
    }
    public class SalonDto
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string OwnerFullName { get; set; }
        public string Name { get; set; }
        public string United { get; set; }
        public int UnitedId { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string FullAddress { get; set; }
        public string Telphone { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }

        public List<Link> Links { get; set; }

    }
}
