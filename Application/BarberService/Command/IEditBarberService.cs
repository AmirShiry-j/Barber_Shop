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
        Task<ResultDto> Execute(User user, EditBarberDto dto);
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

        public async Task<ResultDto> Execute(User user, EditBarberDto dto)
        {
            //Find barber
            var barber = _dbContext.Barbers.Find(dto.Id);

            //Check has barber
            if (barber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "this barberid is not exist"
                };
            }

            //Check baberid is for user authozed
            if (user.Id != barber.UserId)
            {
                return new ResultDto
                {
                    IsSuccess=false,
                    Message="آیدی ارسالی متعلق به یوزر نیست"
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
        public int Id { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int? SalonId { get; set; }
    }
}
