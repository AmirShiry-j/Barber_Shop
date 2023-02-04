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
    public interface IEditBarberService
    {
        Task<ResultDto> Execute(EditBarberDto dto);
    }
    public class EditBarberService : IEditBarberService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public EditBarberService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultDto> Execute(EditBarberDto dto)
        {
            //Find barber
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(dto.UserId)).FirstOrDefault();

            //Check has barber
            if (barber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما قبلا به عنوان آرایشگر ثبت نام نکرده اید"
                };
            }

            //Find Salon
            var salon = _dbContext.Salons.Where(p => p.Id.Equals(dto.SalonId)).FirstOrDefault();
            if (salon == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "سالنی با این آیدی موجود نیست"
                };
            }

            //Update barber infoes
            barber.Description = dto.Description;
            barber.SalonId = dto.SalonId;

            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }

    }

    public class EditBarberDto
    {
        public string Description { get; set; }
        public string UserId { get; set; }
        public int? SalonId { get; set; }
    }
}
