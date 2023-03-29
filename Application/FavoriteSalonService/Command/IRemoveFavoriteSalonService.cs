using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FavoriteSalonService.Command
{
    public interface IRemoveFavoriteSalonService
    {
        Task<ResultDto> Execute(string UserId, int SalonId);
    }
    public class RemoveFavoriteSalonService : IRemoveFavoriteSalonService
    {
        private readonly IDataBaseContext _dbContext;
        public RemoveFavoriteSalonService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultDto> Execute(string UserId, int SalonId)
        {
            //Find Salon
            var Salon = _dbContext.Salons.Find(SalonId);

            //Check has Salon
            if (Salon == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "آرایشگاهی با این آیدی موجود نیست"
                };
            }

            //Chech has before
            var FavoriteSalon = _dbContext.FavoriteSalons.Where(p => p.SalonId.Equals(SalonId) && p.UserId.Equals(UserId)).FirstOrDefault();
            if (FavoriteSalon == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این آرایشگاه قبلا به علاقه مندی های این کاربر اضافه نشده است"
                };
            }

            //Remove in Db
            _dbContext.FavoriteSalons.Remove(FavoriteSalon);
            _dbContext.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
