using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FavoriteBarberService.Command
{
    public interface IRemoveFavoriteBarberService
    {
        Task<ResultDto> Execute(string UserId, int BarberId);
    }
    public class RemoveFavoriteBarberService : IRemoveFavoriteBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public RemoveFavoriteBarberService(IDataBaseContext dbContext)
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
            var FavoriteBarber = _dbContext.FavoriteBarbers.Where(p => p.BarberId.Equals(BarberId) && p.UserId.Equals(UserId)).FirstOrDefault();
            if (FavoriteBarber == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این آرایشگر قبلا به علاقه مندی های این کاربر اضافه نشده است"
                };
            }

            //Remove in Db
            _dbContext.FavoriteBarbers.Remove(FavoriteBarber);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
