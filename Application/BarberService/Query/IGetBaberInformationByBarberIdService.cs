using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BarberService.Query
{
    public interface IGetBaberInformationByBarberIdService
    {
        Task<ResultDto<BarberDto>> Execute(int BarberId);

    }
    public class GetBaberInformationByBarberIdService : IGetBaberInformationByBarberIdService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public GetBaberInformationByBarberIdService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultDto<BarberDto>> Execute(int BarberId)
        {
            //Find Barber
            var barber = _dbContext.Barbers.Where(p => p.Id.Equals(BarberId)).Include(p => p.User).FirstOrDefault();

            //Check has berber
            if (barber == null)
            {
                return new ResultDto<BarberDto>
                {
                    IsSuccess = false,
                    Message = "آرایشگری با آیدی ارسال وجود ندارد"
                };
            }

            //map
            var barberDto = new BarberDto
            {
                BarberId = BarberId,
                FullName = barber.User.FullName,
                Description = barber.Description,
                SalonId = barber.SalonId.Value,
                PhoneNumber = barber.PhoneNumber,
                ImageName=barber.User.ImageName
            };

            return new ResultDto<BarberDto>
            {
                IsSuccess = true,
                Data = barberDto
            };
        }
    }

    public class BarberDto
    {
        public int BarberId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int SalonId { get; set; }
        public string ImageName { get; set; }
        public string UrlImage { get; set; }
    }
}
