using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Salons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FavoriteSalonService.Command
{
    public interface IAddFavoriteSalonService
    {
        Task<ResultDto> Execute(string UserId, int SalonId);
    }
    public class AddFavoriteSalonService : IAddFavoriteSalonService
    {
        private readonly IDataBaseContext _dbContext;
        public AddFavoriteSalonService(IDataBaseContext dbContext)
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
            var hasBefore = _dbContext.FavoriteSalons.Where(p => p.SalonId.Equals(SalonId) && p.UserId.Equals(UserId)).Any();
            if (hasBefore)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این آرایشگاه قبلا به علاقه مندی های این کاربر اضافه شده است"
                };
            }

            //Insert in db
            var newFavorite = new FavoriteSalon
            {
                SalonId = SalonId,
                UserId = UserId
            };

            _dbContext.FavoriteSalons.Add(newFavorite);
            _dbContext.SaveChanges();

            return new ResultDto { IsSuccess = true };
        }
    }
}
