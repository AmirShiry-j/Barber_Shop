using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BarberService.Command
{
    public interface IAddBarberService
    {
        Task<ResultDto<Barber>> Execute(CreateBarberDto dto);
    }

    public class AddBarberService: IAddBarberService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public AddBarberService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultDto<Barber>> Execute(CreateBarberDto dto)
        {
            //has barber before?
            var user = _dbContext.Users.Where(p => p.Id.Equals(dto.UserId)).Include(p => p.Barber).FirstOrDefault();
            if (user.Barber != null)
            {
                return new ResultDto<Barber>
                {
                    IsSuccess = false,
                    Message = "شما قبلا به عنوان آرایشگر ثبت نام کرده اید"
                };
            }


            //map new
            var newBarber = new
                Barber
            {
                UserId = dto.UserId,
                Description = dto.Description,
                SalonId = dto.SalonId,
            };

            //Find Salon
            var salon = _dbContext.Salons.Where(p => p.Id.Equals(dto.SalonId)).FirstOrDefault();
            if (salon == null)
            {
                return new ResultDto<Barber>
                {
                    IsSuccess = false,
                    Message = "سالنی با این آیدی موجود نیست"
                };
            }

            //save in db
            _dbContext.Barbers.Add(newBarber);
            _dbContext.SaveChanges();

            //set forenkey for user
            user.BarberId = newBarber.Id;
            _dbContext.SaveChanges();

            return new ResultDto<Barber>
            {
                IsSuccess = true,
                Data = newBarber
            };
        }
    }
    public class CreateBarberDto
    {
        public string Description { get; set; }

        //Nav
        public string UserId { get; set; }
        public int? SalonId { get; set; }
    }
}
