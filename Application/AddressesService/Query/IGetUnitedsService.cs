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
    public interface IGetUnitedsService
    {
        Task<ResultDto<List<UnitedDto>>> Execute();
    }
    public class GetUnitedsService: IGetUnitedsService
    {
        private readonly IMapper _mapper;
        private readonly IDataBaseContext _dbContext;
        public GetUnitedsService(IDataBaseContext dbContext
            , UserManager<User> userManager
            , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultDto<List<UnitedDto>>> Execute()
        {
            //Get uniteds
            var uniteds = _dbContext.Uniteds.Select(p => new UnitedDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
           
            return new ResultDto<List<UnitedDto>>
            {
                IsSuccess=true,
                Data=uniteds
            };
        }
    }

    public class UnitedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
