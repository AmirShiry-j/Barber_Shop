using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BarberService.Command
{
    public interface IAddBarberService
    {
        Task<ResultDto<Barber>> Execute(User user, CreateBarberDto dto);
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

        public async Task<ResultDto<Barber>> Execute(User user,CreateBarberDto dto)
        {
            //map new
            var newBarber = new
                Barber
            {
                UserId = dto.UserId,
                Description = dto.Description,
                SalonId = dto.SalonId,
            };

            //save in db
            _dbContext.Barbers.Add(newBarber);
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
