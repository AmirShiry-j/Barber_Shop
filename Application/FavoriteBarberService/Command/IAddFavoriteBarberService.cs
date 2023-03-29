using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FavoriteBarberService.Command
{
    public interface IAddFavoriteBarberService
    {
        Task<ResultDto> Execute(string UserId, int BarberId);
    }
    public class AddFavoriteBarberService : IAddFavoriteBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public AddFavoriteBarberService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto> Execute(string UserId, int BarberId)
        {
            //Find barber
            var barber = _dbContext.Barbers.Find(BarberId);

            //Check has barber
            if (barber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "آرایشگری با این آیدی موجود نیست"
                };
            }

            //Chech has before
            var hasBefore = _dbContext.FavoriteBarbers.Where(p => p.BarberId.Equals(BarberId) && p.UserId.Equals(UserId)).Any();
            if (hasBefore)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این آرایشگر قبلا به علاقه مندی های این کاربر اضافه شده است"
                };
            }

            //Insert in db
            var newFavorite = new FavoriteBarber
            {
                BarberId = BarberId,
                UserId = UserId
            };

            _dbContext.FavoriteBarbers.Add(newFavorite);
            _dbContext.SaveChanges();

            return new ResultDto { IsSuccess = true };
        }
    }
}
