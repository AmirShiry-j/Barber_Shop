using Application.Common;
using Application.Interfaces.Contexts;
using Application.SalonsService.Command;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Salons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Application.SalonsService.Query
{
    public interface IGetAllSalonsService
    {
        Task<ResultDto<ResultSearchDto>> Execute(SearchSalonDto searchSalonDto);
    }
    public class GetAllSalonsService : IGetAllSalonsService
    {
        private readonly IDataBaseContext _dbContext;
        private readonly IMapper _mapper;
        public GetAllSalonsService(IDataBaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultDto<ResultSearchDto>> Execute(SearchSalonDto searchSalonDto)
        {
            var prSalon = PredicateBuilder.True<Salon>();

            //Build Predicate
            //Filter SalonName
            if (string.IsNullOrWhiteSpace(searchSalonDto.SalonName) == false)
            {
                prSalon = prSalon.And(x => x.Name.Contains(searchSalonDto.SalonName));
            }
            //Filter ForGender
            if (searchSalonDto.ForGender != null)
            {
                var gender = (Domain.Salons.ForGender)searchSalonDto.ForGender.Value;
                prSalon = prSalon.And(x => x.ForGender == gender);
            }
            //Filter UnitedId
            if (searchSalonDto.UnitedId != null)
            {
                prSalon = prSalon.And(x => x.Address.City.UnitedId == searchSalonDto.UnitedId);
            }
            //Filter CityId
            if (searchSalonDto.CityId != null)
            {
                prSalon = prSalon.And(x => x.Address.CityId == searchSalonDto.CityId);
            }

            var salons = _dbContext.Salons
                .Include(p => p.Address)
                .ThenInclude(p => p.City)
                .ThenInclude(p => p.United)
                .Include(p => p.Owner)
                .Where(prSalon)
                //For Pagination
                .Skip((searchSalonDto.Page.Value - 1) * searchSalonDto.CountInPage.Value)
                .Take(searchSalonDto.CountInPage.Value)
                .Select(p => new SalonMainInfoDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CityId = p.Address.CityId,
                    CityName = p.Address.City.Name,
                    UnitedId = p.Address.City.UnitedId,
                    UnitedName = p.Address.City.United.Name,
                    ForGender = p.ForGender.ToString() == "ForMen" ? Command.ForGender.ForMen : Command.ForGender.ForWomen
                })
                .ToList();

            //For Pagination
            int CountAllItems = _dbContext.Salons.Count();

            return new ResultDto<ResultSearchDto>
            {
                IsSuccess = true,
                Data = new ResultSearchDto
                {
                    Page = searchSalonDto.Page.Value,
                    CountInPage = searchSalonDto.CountInPage.Value,
                    CountAllItems = CountAllItems,
                    Salons = salons
                }
            };
        }
    }
    public class SearchSalonDto
    {
        public string? SalonName { get; set; }
        public int? CityId { get; set; }
        public int? UnitedId { get; set; }
        public int? Page { get; set; } = 1;
        public int? CountInPage { get; set; } = 10;
        public Application.SalonsService.Command.ForGender? ForGender { get; set; }
    }
    public class ResultSearchDto
    {
        public int Page { get; set; }
        public int CountInPage { get; set; }
        public int CountAllItems { get; set; }
        public List<SalonMainInfoDto> Salons { get; set; }
    }
    public class SalonMainInfoDto
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int UnitedId { get; set; }
        public string UnitedName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Command.ForGender ForGender { get; set; }
        public Link Link { get; set; }
    }
}
