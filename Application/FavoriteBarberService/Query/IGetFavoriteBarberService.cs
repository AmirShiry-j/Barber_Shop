using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FavoriteBarberService.Query
{

    public interface IGetFavoriteBarberService
    {
        Task<ResultDto<List<FavoriteBarberDto>>> Execute(string UserId);

    }
    public class GetFavoriteBarberService : IGetFavoriteBarberService
    {
        private readonly IDataBaseContext _dbContext;
        public GetFavoriteBarberService(IDataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<List<FavoriteBarberDto>>> Execute(string UserId)
        {
            //Get FavoriteBarbers
            var FavoriteBarbers = _dbContext.FavoriteBarbers
                .Where(p=>p.UserId.Equals(UserId))
                .Include(p => p.Barber)
                .ThenInclude(p => p.User)
                .Select(p => new FavoriteBarberDto
                {
                    Id = p.Id,
                    BarberId = p.BarberId,
                    FullName = p.Barber.User.FullName,
                    NameProfileImage = p.Barber.User.ImageName
                })
                .OrderByDescending(p => p.Id)
                .ToList();

            return new ResultDto<List<FavoriteBarberDto>>
            {
                IsSuccess = true,
                Data = FavoriteBarbers
            };
        }
    }
    public class FavoriteBarberDto
    {
        public int Id { get; set; }
        public int BarberId { get; set; }
        public string FullName { get; set; }
        public string NameProfileImage { get; set; }
        public string UrlProfileImage { get; set; }
        public string UrlProfileBarber { get; set; }
        public Link Link { get; set; }
    }
}
