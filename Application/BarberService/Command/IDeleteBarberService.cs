using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BarberService.Command
{
    public interface IDeleteBarberService
    {
        Task<ResultDto> Execute(User user);
    }
    public class DeleteBarberService : IDeleteBarberService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public DeleteBarberService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultDto> Execute(User user)
        {
            //get barber
            var barber = _dbContext.Barbers.Where(p => p.UserId.Equals(user.Id)).FirstOrDefault();

            if (barber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "شما قبلا به عنوان آرایشگر ثبت نام نکرده اید"
                };
            }

            //delete in db
            _dbContext.Barbers.Remove(barber);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }

    }

}
