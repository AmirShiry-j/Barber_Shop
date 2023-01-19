using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AddressesService.Query
{
    public interface IGetCitiesService
    {
        Task<ResultDto<List<CityDto>>> Execute(int UnitedId);
    }
    public class GetCitiesService : IGetCitiesService
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseContext _dbContext;
        public GetCitiesService(IDataBaseContext dbContext
            , UserManager<User> userManager
            , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultDto<List<CityDto>>> Execute(int UnitedId)
        {
            //Get cities by UnitedId
            var cities = _dbContext.Cities.Where(p => p.UnitedId == UnitedId)
                .Select(p => new CityDto
                {
                    Name = p.Name,
                    Id = p.Id
                })
                .OrderBy(p=>p.Name)
                .ToList();

            if (cities.Any())//if unitedId was reall
            {
                return new ResultDto<List<CityDto>>
                {
                    IsSuccess = true,
                    Data = cities
                };
            }
            else
            {
                return new ResultDto<List<CityDto>>
                {
                    IsSuccess = false,
                    Message = "استانی با آیدی ارسال شده موجود نیست"
                };
            }
        }
    }
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
