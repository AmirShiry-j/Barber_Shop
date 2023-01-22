using Application.Common;
using Application.Interfaces.Contexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SalonsService.Query
{
    public interface IGetAllSalonsService
    {
        Task<ResultDto<List<SalonMainInfoDto>>> Execute();
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
        public async Task<ResultDto<List<SalonMainInfoDto>>> Execute()
        {
            var salons = _dbContext.Salons.Include(p => p.Address).Include(p => p.Owner).Select(p => new SalonMainInfoDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            }).ToList();

            return new ResultDto<List<SalonMainInfoDto>>
            {
                IsSuccess = true,
                Data = salons
            };
        }
    }
    public class SalonMainInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Link Link { get; set; }
    }
}
