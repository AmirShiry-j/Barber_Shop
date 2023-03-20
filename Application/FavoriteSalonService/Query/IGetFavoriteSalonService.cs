using Application.Common;
using Application.FavoriteBarberService.Query;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FavoriteSalonService.Query
{
    public interface IGetFavoriteSalonService
    {
        Task<ResultDto<List<FavoriteSalonDto>>> Execute(string UserId);
    }
    public class GetFavoriteSalonService : IGetFavoriteSalonService
    {
        private readonly IDataBaseContext _dbContext;
        public GetFavoriteSalonService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<List<FavoriteSalonDto>>> Execute(string UserId)
        {
            //Get FavoriteSalons
            var favoriteSalons = _dbContext.FavoriteSalons
                .Include(p => p.Salon)
                .ThenInclude(p => p.SalonImages)
                .Select(p => new FavoriteSalonDto
                {
                    Id = p.Id,
                    SalonId = p.SalonId,
                    Name = p.Salon.Name,
                    NameSalonImage = p.Salon.SalonImages.FirstOrDefault() == null ? null : p.Salon.SalonImages.FirstOrDefault().Name.ToString(),
                })
                .OrderByDescending(p => p.Id)
                .ToList();

            return new ResultDto<List<FavoriteSalonDto>>
            {
                IsSuccess = true,
                Data = favoriteSalons
            };
        }
    }
    public class FavoriteSalonDto
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string Name { get; set; }
        public string NameSalonImage { get; set; }
        public string UrlSalonImage { get; set; }
        public string UrlSalon { get; set; }
    }
}
