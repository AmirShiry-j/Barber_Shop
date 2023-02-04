using Application.Common;
using Application.Interfaces.Contexts;
using Application.SalonsService.Query;
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
    public interface IGetBarberInfoByUserIdService
    {
        Task<ResultDto<BarberInfoDto>> Execute(string UserId);

    }
    public class GetBarberInfoByUserIdService : IGetBarberInfoByUserIdService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public GetBarberInfoByUserIdService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultDto<BarberInfoDto>> Execute(string UserId)
        {
            //Find user and barber infoes
            var user = _dbContext.Users.Where(p => p.Id.Equals(UserId)).Include(p => p.Barber).FirstOrDefault();

            //Check user is Barber
            if (user.Barber == null)
            {
                return new ResultDto<BarberInfoDto>
                {
                    IsSuccess = false,
                    Message = "شما قبلا به عنوان آرایشگر ثبت نام نکرده اید"
                };
            }

            //Map
            var barberInfo = new BarberInfoDto
            {
                BarberId = user.BarberId.Value,
                Description = user.Barber.Description,
                SalonId = user.Barber.SalonId.Value
            };

            return new ResultDto<BarberInfoDto>
            {
                IsSuccess = true,
                Data = barberInfo,
            };
        }
    }
    public class BarberInfoDto
    {
        public int BarberId { get; set; }
        public string Description { get; set; }
        public int SalonId { get; set; }
        public List<Link> Links { get; set; }
    }
}
